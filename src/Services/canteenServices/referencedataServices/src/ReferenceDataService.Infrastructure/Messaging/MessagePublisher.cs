using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using ReferenceDataService.Application.Interfaces;

namespace ReferenceDataService.Infrastructure.Messaging;

public class MessagePublisher : IMessagePublisher
{
    private readonly RabbitMqConnection _connection;

    public MessagePublisher(RabbitMqConnection connection)
    {
        _connection = connection;
    }

    public async Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken cancellationToken = default)
    {
        var channel = await _connection.GetChannelAsync();

        await channel.ExchangeDeclareAsync(exchange, ExchangeType.Topic, durable: true, cancellationToken: cancellationToken);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var props = new BasicProperties
        {
            ContentType = "application/json",
            DeliveryMode = DeliveryModes.Persistent
        };

        await channel.BasicPublishAsync(exchange, routingKey, false, props, body, cancellationToken);
    }
}
