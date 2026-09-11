using FleetPulse.Application.Abstractions.Messaging;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Text;
using System.Text.Json;

namespace FleetPulse.WebAPI.Messaging;

public class RabbitMqEventPublisher(
    IConfiguration configuration,
    RabbitMqConnectionManager connectionManager,
    ILogger<RabbitMqEventPublisher> logger
) : IEventPublisher
{
    private readonly IConfiguration _configuration = configuration;
    private readonly RabbitMqConnectionManager _connectionManager = connectionManager;
    private readonly ILogger<RabbitMqEventPublisher> _logger = logger;

    public async Task PublishAsync<T>(T message)
    {
        var exchangeName =
            _configuration["RabbitMQ:ExchangeName"]!;

        var queueName =
            _configuration["RabbitMQ:GeofenceViolationQueue"]!;

        var routingKey =
            _configuration["RabbitMQ:GeofenceViolationRoutingKey"]!;

        try
        {
            var connection =
                await _connectionManager.GetConnectionAsync();

            var channelOptions = new CreateChannelOptions(
                publisherConfirmationsEnabled: true,
                publisherConfirmationTrackingEnabled: true
            );

            await using var channel =
                await connection.CreateChannelAsync(channelOptions);

            await channel.ExchangeDeclareAsync(
                exchange: exchangeName,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false
            );

            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false
            );

            await channel.QueueBindAsync(
                queue: queueName,
                exchange: exchangeName,
                routingKey: routingKey
            );

            var json =
                JsonSerializer.Serialize(message);

            var body =
                Encoding.UTF8.GetBytes(json);

            await channel.BasicPublishAsync(
                exchange: exchangeName,
                routingKey: routingKey,
                mandatory: true,
                body: body
            );
        }
        catch (BrokerUnreachableException)
        {
            _logger.LogWarning(
         "RabbitMQ Publisher bağlantısı kurulamadı. Event publish edilemedi. Host: {Host}, Port: {Port}",
         _configuration["RabbitMQ:HostName"],
         _configuration["RabbitMQ:Port"]
     );

            throw;
        }
        catch (PublishReturnException ex)
        {
            _logger.LogError(
                ex,
                "RabbitMQ mesajı route edilemedi. Exchange: {Exchange}, RoutingKey: {RoutingKey}",
                exchangeName,
                routingKey
            );

            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "RabbitMQ publish sırasında beklenmeyen hata oluştu. Exchange: {Exchange}, RoutingKey: {RoutingKey}",
                exchangeName,
                routingKey
            );

            throw;
        }
    }
}