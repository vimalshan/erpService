namespace InvoiceProcessing.Domain.Interfaces;

public interface IMessagePublisher
{
    Task PublishAsync<T>(string exchange, string routingKey, T message, CancellationToken ct = default) where T : class;
}
