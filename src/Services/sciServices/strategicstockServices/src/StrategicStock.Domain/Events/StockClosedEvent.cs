using StrategicStock.Domain.Common;

namespace StrategicStock.Domain.Events;

public sealed record StockClosedEvent(
    int StrategicStockId,
    string ClosureDate) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
