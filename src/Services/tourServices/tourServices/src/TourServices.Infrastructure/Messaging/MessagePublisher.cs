using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace TourServices.Infrastructure.Messaging;

public sealed class MessagePublisher : IDisposable
{
    private readonly IChannel _channel;
    private readonly IConnection _connection;
    private bool _disposed;

    public MessagePublisher(IConnection connection)
    {
        _connection = connection;
        _channel = connection.CreateChannelAsync().GetAwaiter().GetResult();
    }

    public async Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default)
    {
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        await _channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true, cancellationToken: ct);
        await _channel.BasicPublishAsync(
            exchange: exchange,
            routingKey: routingKey,
            mandatory: false,
            body: body,
            cancellationToken: ct);
    }

    public void Dispose()
    {
        if (_disposed) return;
        _channel.CloseAsync().GetAwaiter().GetResult();
        _channel.Dispose();
        _disposed = true;
    }
}
