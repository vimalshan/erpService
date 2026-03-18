namespace AuditService.Application.Interfaces;

public interface IMessagePublisher
{
    Task PublishAsync<T>(string exchangeName, string routingKey, T message, CancellationToken cancellationToken = default) where T : class;
}
