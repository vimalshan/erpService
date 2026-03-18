namespace ApprovalService.Infrastructure.Messaging;

using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using ApprovalService.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

/// <summary>
/// RabbitMQ Message Publisher
/// </summary>
public class RabbitMqMessagePublisher : IMessagePublisher
{
    private readonly IConnection _connection;
    private readonly ILogger<RabbitMqMessagePublisher> _logger;

    public RabbitMqMessagePublisher(IConnection connection, ILogger<RabbitMqMessagePublisher> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public async Task PublishAsync<T>(string routingKey, T message) where T : class
    {
        try
        {
            using var channel = _connection.CreateModel();

            // Declare exchange
            channel.ExchangeDeclare(
                exchange: "approval-service",
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.ContentType = "application/json";
            properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            channel.BasicPublish(
                exchange: "approval-service",
                routingKey: routingKey,
                basicProperties: properties,
                body: body);

            _logger.LogInformation("Published message to RabbitMQ with routing key {RoutingKey}", routingKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing message to RabbitMQ with routing key {RoutingKey}", routingKey);
            throw;
        }
    }
}

/// <summary>
/// RabbitMQ Connection Factory
/// </summary>
public static class RabbitMqConnectionFactory
{
    public static IConnection CreateConnection(IConfiguration configuration)
    {
        var factory = new ConnectionFactory()
        {
            HostName = configuration["RabbitMq:HostName"] ?? "localhost",
            UserName = configuration["RabbitMq:UserName"] ?? "guest",
            Password = configuration["RabbitMq:Password"] ?? "guest",
            Port = int.Parse(configuration["RabbitMq:Port"] ?? "5672"),
            VirtualHost = configuration["RabbitMq:VirtualHost"] ?? "/"
        };

        return factory.CreateConnection();
    }
}
