using System.Text;
using System.Text.Json;
using ArchiveService.Application.Interfaces;
using RabbitMQ.Client;

namespace ArchiveService.Infrastructure.Messaging;

public class RabbitMqPublisher(IConnection connection) : IMessagePublisher, IAsyncDisposable
{
    private IChannel? _channel;

    private async Task<IChannel> GetChannelAsync(CancellationToken ct = default)
    {
        _channel ??= await connection.CreateChannelAsync(cancellationToken: ct);
        return _channel;
    }

    public async Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default)
    {
        var channel = await GetChannelAsync(ct);
        await channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true, cancellationToken: ct);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
        var props = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent
        };

        await channel.BasicPublishAsync(exchange, routingKey, mandatory: false, basicProperties: props, body: body, cancellationToken: ct);
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel is not null)
            await _channel.CloseAsync();
        GC.SuppressFinalize(this);
    }
}
