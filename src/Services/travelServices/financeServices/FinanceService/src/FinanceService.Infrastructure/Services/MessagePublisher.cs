using System.Text;
using System.Text.Json;
using FinanceService.Application.Common.Interfaces;
using RabbitMQ.Client;

namespace FinanceService.Infrastructure.Services;

public class MessagePublisher : IMessagePublisher
{
    private readonly IConnection _connection;

    public MessagePublisher(IConnection connection)
    {
        _connection = connection;
    }

    public async Task PublishAsync<T>(T message, string exchangeName, string routingKey, CancellationToken ct = default)
    {
        using var channel = await _connection.CreateChannelAsync(cancellationToken: ct);

        await channel.ExchangeDeclareAsync(
            exchange: exchangeName,
            type: ExchangeType.Topic,
            durable: true,
            cancellationToken: ct);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        var properties = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent
        };

        await channel.BasicPublishAsync(
            exchange: exchangeName,
            routingKey: routingKey,
            mandatory: false,
            basicProperties: properties,
            body: body,
            cancellationToken: ct);
    }
}
