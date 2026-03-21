namespace WarehouseStructure.Domain.Events;

public sealed class WarehouseUpdatedEvent : IDomainEvent
{
    public int WarehouseId { get; }
    public string Code { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public WarehouseUpdatedEvent(int warehouseId, string code)
    {
        WarehouseId = warehouseId;
        Code = code;
    }
}
