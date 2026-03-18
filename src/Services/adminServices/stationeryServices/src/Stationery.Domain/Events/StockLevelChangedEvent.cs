using Stationery.Domain.Common;
using Stationery.Domain.Entities;

namespace Stationery.Domain.Events;

public class StockLevelChangedEvent : DomainEvent
{
    public long StationaryId { get; init; }
    public string Description { get; init; } = string.Empty;
    public long NewStock { get; init; }
    public long ReorderLevel { get; init; }
    public bool IsBelowReorderLevel { get; init; }

    private StockLevelChangedEvent() { }

    public StockLevelChangedEvent(StationaryMaster item, long newStock)
    {
        StationaryId = item.Id;
        Description = item.Description;
        NewStock = newStock;
        ReorderLevel = item.ReorderLevel ?? 0;
        IsBelowReorderLevel = newStock < (item.ReorderLevel ?? 0);
    }
}
