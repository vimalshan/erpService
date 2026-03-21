using InventoryService.Domain.Common;

namespace InventoryService.Domain.Events;

public sealed record StockLevelChangedEvent(
    int ProductId,
    int WarehouseId,
    int BinId,
    decimal PreviousQuantity,
    decimal NewQuantity) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
