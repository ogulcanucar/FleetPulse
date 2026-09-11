using FleetPulse.Application.Events.Geofences;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System.Text;
using System.Text.Json;

namespace FleetPulse.WebAPI.Workers
{
    public class GeofenceViolationConsumer(
        IConfiguration configuration,
        ILogger<GeofenceViolationConsumer> logger) : BackgroundService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<GeofenceViolationConsumer> _logger = logger;

        private const int MaxRetryCount = 3;

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ConsumeAsync(stoppingToken);
                }
                catch (BrokerUnreachableException)
                {
                    _logger.LogWarning(
                        "RabbitMQ kapalı veya erişilemiyor. " +
                        "5 saniye sonra tekrar bağlantı denenecek. " +
                        "Host: {Host}, Port: {Port}",
                        _configuration["RabbitMQ:HostName"],
                        _configuration["RabbitMQ:Port"]
                    );

                    await Task.Delay(
                        TimeSpan.FromSeconds(5),
                        stoppingToken
                    );
                }
                catch (OperationCanceledException)
                    when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
            }
        }

        private async Task ConsumeAsync(
            CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName =
                    _configuration["RabbitMQ:HostName"]!,

                Port =
                    int.Parse(
                        _configuration["RabbitMQ:Port"]!
                    ),

                UserName =
                    _configuration["RabbitMQ:UserName"]!,

                Password =
                    _configuration["RabbitMQ:Password"]!
            };

            await using var connection =
                await factory.CreateConnectionAsync(
                    stoppingToken
                );

            await using var channel =
                await connection.CreateChannelAsync(
                    cancellationToken: stoppingToken
                );

            var exchangeName =
                _configuration["RabbitMQ:ExchangeName"]!;

            var routingKey =
                _configuration[
                    "RabbitMQ:GeofenceViolationRoutingKey"
                ]!;

            var queueName =
                _configuration[
                    "RabbitMQ:GeofenceViolationQueue"
                ]!;

            var retryQueueName =
                _configuration[
                    "RabbitMQ:GeofenceViolationRetryQueue"
                ]!;

            var deadLetterQueueName =
                _configuration[
                    "RabbitMQ:GeofenceViolationDeadLetterQueue"
                ]!;

            var retryQueueArguments =
                new Dictionary<string, object?>
                {
                    ["x-message-ttl"] = 5000,

                    ["x-dead-letter-exchange"] =
                        exchangeName,

                    ["x-dead-letter-routing-key"] =
                        routingKey
                };

            await channel.QueueDeclareAsync(
                queue: retryQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: retryQueueArguments,
                cancellationToken: stoppingToken
            );

            await channel.QueueDeclareAsync(
                queue: deadLetterQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                cancellationToken: stoppingToken
            );

            var consumer =
                new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync +=
                async (sender, args) =>
                {
                    var retryCount = 0;

                    if (
                        args.BasicProperties.Headers
                            is not null
                        &&
                        args.BasicProperties.Headers
                            .TryGetValue(
                                "x-retry-count",
                                out var retryHeader
                            )
                    )
                    {
                        retryCount =
                            retryHeader switch
                            {
                                int value =>
                                    value,

                                long value =>
                                    (int)value,

                                byte[] value
                                    when int.TryParse(
                                        Encoding.UTF8
                                            .GetString(value),
                                        out var parsedValue
                                    )
                                    =>
                                        parsedValue,

                                _ => 0
                            };
                    }

                    var body =
                        args.Body.ToArray();

                    _logger.LogInformation(
                        "RabbitMQ Consumer -> Geofence ihlali mesajı alındı. RetryCount: {RetryCount}",
                        retryCount
                    );

                    try
                    {
                        var json =
                            Encoding.UTF8.GetString(
                                body
                            );

                        var violationEvent =
                            JsonSerializer
                                .Deserialize<
                                    GeofenceViolationCreatedEvent
                                >(json);

                        if (violationEvent is not null)
                        {
                            _logger.LogWarning(
                                "RabbitMQ Consumer -> Geofence ihlali alındı. AssetId: {AssetId}, Mesafe: {Distance:F2}",
                                violationEvent.AssetId,
                                violationEvent
                                    .DistanceInMeters
                            );

                            await channel
                                .BasicAckAsync(
                                    deliveryTag:
                                        args.DeliveryTag,

                                    multiple: false
                                );
                        }
                        else
                        {
                            _logger.LogError(
                                "RabbitMQ Consumer -> Event null döndü. Mesaj DLQ'ya gönderiliyor. DeliveryTag: {DeliveryTag}",
                                args.DeliveryTag
                            );

                            await channel
                                .BasicPublishAsync(
                                    exchange: "",
                                    routingKey:
                                        deadLetterQueueName,
                                    body: body
                                );

                            await channel
                                .BasicAckAsync(
                                    deliveryTag:
                                        args.DeliveryTag,

                                    multiple: false
                                );
                        }
                    }
                    catch (JsonException ex)
                    {
                        _logger.LogError(
                            ex,
                            "RabbitMQ Consumer -> Mesaj deserialize edilemedi. Mesaj DLQ'ya gönderiliyor. DeliveryTag: {DeliveryTag}",
                            args.DeliveryTag
                        );

                        await channel.BasicPublishAsync(
                            exchange: "",
                            routingKey:
                                deadLetterQueueName,
                            body: body
                        );

                        await channel.BasicAckAsync(
                            deliveryTag:
                                args.DeliveryTag,

                            multiple: false
                        );
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(
                            ex,
                            "RabbitMQ Consumer -> Mesaj işlenirken hata oluştu. RetryCount: {RetryCount}",
                            retryCount
                        );

                        if (
                            retryCount >=
                            MaxRetryCount
                        )
                        {
                            _logger.LogError(
                                "RabbitMQ Consumer -> Maksimum retry sayısına ulaşıldı. Mesaj DLQ'ya gönderiliyor. DeliveryTag: {DeliveryTag}",
                                args.DeliveryTag
                            );

                            await channel
                                .BasicPublishAsync(
                                    exchange: "",
                                    routingKey:
                                        deadLetterQueueName,
                                    body: body
                                );

                            await channel
                                .BasicAckAsync(
                                    deliveryTag:
                                        args.DeliveryTag,

                                    multiple: false
                                );
                        }
                        else
                        {
                            var properties =
                                new BasicProperties
                                {
                                    Headers =
                                        new Dictionary<
                                            string,
                                            object?
                                        >
                                        {
                                            [
                                                "x-retry-count"
                                            ] =
                                                retryCount
                                                + 1
                                        }
                                };

                            _logger.LogWarning(
                                "RabbitMQ Consumer -> Mesaj Retry Queue'ya gönderiliyor. Yeni RetryCount: {RetryCount}",
                                retryCount + 1
                            );

                            await channel
                                .BasicPublishAsync(
                                    exchange: "",
                                    routingKey:
                                        retryQueueName,
                                    mandatory: false,
                                    basicProperties:
                                        properties,
                                    body: body
                                );

                            await channel
                                .BasicAckAsync(
                                    deliveryTag:
                                        args.DeliveryTag,

                                    multiple: false
                                );
                        }
                    }
                };

            await channel.BasicConsumeAsync(
                queue: queueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stoppingToken
            );

            _logger.LogInformation(
                "RabbitMQ Consumer bağlantısı başarılı. Mesajlar dinleniyor."
            );

            await Task.Delay(
                Timeout.Infinite,
                stoppingToken
            );
        }
    }
}