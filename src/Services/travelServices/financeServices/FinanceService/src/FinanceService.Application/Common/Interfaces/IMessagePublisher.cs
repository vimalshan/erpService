namespace FinanceService.Application.Common.Interfaces;

public interface IMessagePublisher
{
    Task PublishAsync<T>(T message, string exchangeName, string routingKey, CancellationToken ct = default);
}
