using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using Microsoft.Extensions.Logging;

namespace EmployeeRelations.Infrastructure.Messaging;

public interface IMessagePublisher
{
    Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default);
}

public class RabbitMqPublisher : IMessagePublisher, IDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    private readonly ILogger<RabbitMqPublisher> _logger;

    public RabbitMqPublisher(IConnection connection, ILogger<RabbitMqPublisher> logger)
    {
        _connection = connection;
        _logger = logger;
        _channel = connection.CreateChannelAsync().GetAwaiter().GetResult();
    }

    public async Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default)
    {
        await _channel.ExchangeDeclareAsync(exchange, RabbitMQ.Client.ExchangeType.Topic, durable: true, cancellationToken: ct);
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        var props = new BasicProperties { ContentType = "application/json", DeliveryMode = DeliveryModes.Persistent };

        await _channel.BasicPublishAsync(
            exchange: exchange,
            routingKey: routingKey,
            mandatory: false,
            basicProperties: props,
            body: body,
            cancellationToken: ct);

        _logger.LogInformation("Published message to exchange '{Exchange}' with key '{Key}'", exchange, routingKey);
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
