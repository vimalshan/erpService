namespace ApprovalGroup.Domain.Interfaces;

public interface IMessagePublisher
{
    Task PublishAsync<T>(T message, string routingKey, CancellationToken ct = default);
}
