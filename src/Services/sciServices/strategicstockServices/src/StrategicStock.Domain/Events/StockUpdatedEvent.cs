using StrategicStock.Domain.Common;

namespace StrategicStock.Domain.Events;

public sealed record StockUpdatedEvent(
    int StrategicStockId,
    long? MaxQty,
    long? FilledQty) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
