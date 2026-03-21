namespace SalesOrderService.Domain.Interfaces;

/// <summary>Abstraction for publishing integration events to the message bus.</summary>
public interface IEventBus
{
    Task PublishAsync<T>(T message, CancellationToken ct = default) where T : class;
}
