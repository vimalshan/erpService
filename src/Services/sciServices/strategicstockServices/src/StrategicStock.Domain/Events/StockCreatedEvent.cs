using StrategicStock.Domain.Common;

namespace StrategicStock.Domain.Events;

public sealed record StockCreatedEvent(
    int StrategicStockId,
    int SciItemId,
    int? CompanyUnitId,
    string? StockType,
    long? MaxQty) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
