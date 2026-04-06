namespace PayrollServices.Infrastructure.Messaging;

public interface IMessageBrokerService
{
    Task PublishMessageAsync<T>(string exchange, string routingKey, T message, CancellationToken cancellationToken = default);
    Task StartConsumingAsync(CancellationToken cancellationToken = default);
}
