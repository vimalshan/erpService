namespace CanteenUnit.Infrastructure.Messaging;

public interface IRabbitMQPublisher
{
    Task PublishAsync<T>(T message, string exchange, string routingKey, CancellationToken ct = default);
}
