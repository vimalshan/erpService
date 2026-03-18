using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace LovService.Infrastructure.Messaging;

public class RabbitMQPublisher(IConnection connection, ILogger<RabbitMQPublisher> logger) : IDisposable
{
    private readonly IChannel? _channel = null;

    public async Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default)
    {
        try
        {
            await using var channel = await connection.CreateChannelAsync(cancellationToken: ct);
            await channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true, cancellationToken: ct);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var props = new BasicProperties { ContentType = "application/json", DeliveryMode = DeliveryModes.Persistent };
            await channel.BasicPublishAsync(exchange, routingKey, false, props, body, ct);

            logger.LogInformation("Published message to exchange '{Exchange}' routing key '{RoutingKey}'", exchange, routingKey);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to publish message to RabbitMQ");
            throw;
        }
    }

    public void Dispose() => _channel?.Dispose();
}
