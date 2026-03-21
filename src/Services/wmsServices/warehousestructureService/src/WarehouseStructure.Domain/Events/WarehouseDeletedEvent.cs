namespace WarehouseStructure.Domain.Events;

public sealed class WarehouseDeletedEvent : IDomainEvent
{
    public int WarehouseId { get; }
    public string Code { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public WarehouseDeletedEvent(int warehouseId, string code)
    {
        WarehouseId = warehouseId;
        Code = code;
    }
}
