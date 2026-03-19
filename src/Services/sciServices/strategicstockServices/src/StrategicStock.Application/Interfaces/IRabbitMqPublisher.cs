namespace StrategicStock.Application.Interfaces;

public interface IRabbitMqPublisher
{
    Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default);
}
