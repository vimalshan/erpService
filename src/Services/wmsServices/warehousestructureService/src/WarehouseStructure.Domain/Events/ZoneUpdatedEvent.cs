namespace WarehouseStructure.Domain.Events;

public sealed class ZoneUpdatedEvent : IDomainEvent
{
    public int ZoneId { get; }
    public int WarehouseId { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public ZoneUpdatedEvent(int zoneId, int warehouseId)
    {
        ZoneId = zoneId;
        WarehouseId = warehouseId;
    }
}
