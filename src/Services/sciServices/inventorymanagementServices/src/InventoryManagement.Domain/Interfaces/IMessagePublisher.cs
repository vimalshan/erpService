namespace InventoryManagement.Domain.Interfaces;

public interface IMessagePublisher
{
    Task PublishAsync<T>(string queueName, T message, CancellationToken ct = default) where T : class;
}
