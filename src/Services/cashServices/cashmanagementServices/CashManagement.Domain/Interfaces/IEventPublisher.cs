namespace CashManagement.Domain.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync<T>(string routingKey, T message);
}
