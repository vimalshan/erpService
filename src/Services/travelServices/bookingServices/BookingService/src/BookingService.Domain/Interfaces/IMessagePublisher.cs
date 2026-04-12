namespace BookingService.Domain.Interfaces;

public interface IMessagePublisher
{
    Task PublishAsync<T>(string routingKey, T message, CancellationToken ct = default);
}
