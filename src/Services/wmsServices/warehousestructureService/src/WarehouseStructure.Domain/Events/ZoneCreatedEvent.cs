namespace WarehouseStructure.Domain.Events;

public sealed class ZoneCreatedEvent : IDomainEvent
{
    public int ZoneId { get; }
    public int WarehouseId { get; }
    public string Code { get; }
    public string ZoneType { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public ZoneCreatedEvent(int zoneId, int warehouseId, string code, string zoneType)
    {
        ZoneId = zoneId;
        WarehouseId = warehouseId;
        Code = code;
        ZoneType = zoneType;
    }
}
