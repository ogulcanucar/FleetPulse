using FleetPulse.Application.Abstractions.Messaging;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace FleetPulse.WebAPI.Messaging;

public class RabbitMqEventPublisher(
    IConfiguration configuration,
      RabbitMqConnectionManager connectionManager
    ) : IEventPublisher
{
    private readonly IConfiguration _configuration = configuration;
    private readonly RabbitMqConnectionManager _connectionManager = connectionManager;
    public async Task PublishAsync<T>(T message)
    {
       
        var connection = await _connectionManager.GetConnectionAsync();

        await using var channel = await connection.CreateChannelAsync();


        var exchangeName = _configuration["RabbitMQ:ExchangeName"]!;
        var queueName = _configuration["RabbitMQ:GeofenceViolationQueue"]!;
        var routingKey = _configuration["RabbitMQ:GeofenceViolationRoutingKey"]!;

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
            autoDelete: false);

        await channel.QueueBindAsync(
            queue: queueName,
            exchange: exchangeName,
            routingKey: routingKey
        );


        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(
            exchange: exchangeName,
            routingKey: routingKey,
            body: body
        );



    }
}