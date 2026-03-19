using MenuAndSecurityService.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace MenuAndSecurityService.Infrastructure.Messaging;

public class MessagePublisher : IMessagePublisher
{
    private readonly RabbitMqConnection _connection;
    private readonly ILogger<MessagePublisher> _logger;

    public MessagePublisher(RabbitMqConnection connection, ILogger<MessagePublisher> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public async Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default)
    {
        try
        {
            var channel = await _connection.GetChannelAsync();
            await channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true, cancellationToken: ct);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = new BasicProperties
            {
                ContentType = "application/json",
                DeliveryMode = DeliveryModes.Persistent,
                Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            };

            await channel.BasicPublishAsync(exchange, routingKey, false, properties, body, ct);

            _logger.LogInformation("Published message to {Exchange}/{RoutingKey}", exchange, routingKey);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish message to {Exchange}/{RoutingKey}", exchange, routingKey);
        }
    }
}
