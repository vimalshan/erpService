using InventoryService.Domain.Common;

namespace InventoryService.Domain.Events;

public sealed record InventoryTransferredEvent(
    int ProductId,
    int FromWarehouseId,
    int FromBinId,
    int ToWarehouseId,
    int ToBinId,
    decimal Quantity) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
