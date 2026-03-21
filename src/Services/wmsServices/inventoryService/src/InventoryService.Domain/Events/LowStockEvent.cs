using InventoryService.Domain.Common;

namespace InventoryService.Domain.Events;

public sealed record LowStockEvent(
    int ProductId,
    int WarehouseId,
    int BinId,
    decimal CurrentQuantity,
    int ReorderLevel) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
