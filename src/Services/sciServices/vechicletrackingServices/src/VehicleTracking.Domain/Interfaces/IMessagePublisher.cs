namespace VehicleTracking.Domain.Interfaces;

public interface IMessagePublisher
{
    Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default);
}
