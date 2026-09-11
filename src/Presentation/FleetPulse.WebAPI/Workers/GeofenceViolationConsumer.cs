
using FleetPulse.Application.Events.Geofences;
using Microsoft.IdentityModel.Tokens.Experimental;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace FleetPulse.WebAPI.Workers
{
    public class GeofenceViolationConsumer(IConfiguration configuration, ILogger<GeofenceViolationConsumer> logger) : BackgroundService
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<GeofenceViolationConsumer> _logger = logger;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMQ:HostName"]!,
                Port = int.Parse(_configuration["RabbitMQ:Port"]!),
                UserName = _configuration["RabbitMQ:UserName"]!,
                Password = _configuration["RabbitMQ:Password"]!
            };
            await using var connection = await factory.CreateConnectionAsync();
            await using var channel = await connection.CreateChannelAsync();

            var queueName = _configuration["RabbitMQ:GeofenceViolationQueue"]!;

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (sender, args) =>
            {
                var body = args.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var violationEvent = JsonSerializer.Deserialize<GeofenceViolationCreatedEvent>(json);

                if (violationEvent is not null)
                {
                    _logger.LogWarning("RabbitMQ Consumer -> Geofence ilhali alındı. AssetId: {AssetId}, Mesafe: {Distance:F2}",
                        violationEvent.AssetId,
                        violationEvent.DistanceInMeters
                        );
                    await channel.BasicAckAsync(
                        deliveryTag: args.DeliveryTag,
                        multiple: false
                    );
                }

            };
            await channel.BasicConsumeAsync(
                queue: queueName,
                autoAck: false,
                consumer: consumer
            );

            await Task.Delay(Timeout.Infinite, stoppingToken);



        }
    }
}
