namespace ReviewService.Domain.Interfaces;

public interface IMessageBusService
{
    Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken cancellationToken = default) where T : class;
}
