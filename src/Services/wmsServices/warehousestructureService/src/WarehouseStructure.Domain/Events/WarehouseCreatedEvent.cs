namespace WarehouseStructure.Domain.Events;

public sealed class WarehouseCreatedEvent : IDomainEvent
{
    public int WarehouseId { get; }
    public string Code { get; }
    public string Name { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public WarehouseCreatedEvent(int warehouseId, string code, string name)
    {
        WarehouseId = warehouseId;
        Code = code;
        Name = name;
    }
}
