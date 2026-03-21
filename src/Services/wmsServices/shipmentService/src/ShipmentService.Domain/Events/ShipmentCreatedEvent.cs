using ShipmentService.Domain.Common;

namespace ShipmentService.Domain.Events;

public sealed class ShipmentCreatedEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public string ShipmentNumber { get; }
    public int CustomerId { get; }
    public int WarehouseId { get; }

    public ShipmentCreatedEvent(string shipmentNumber, int customerId, int warehouseId)
    {
        ShipmentNumber = shipmentNumber;
        CustomerId = customerId;
        WarehouseId = warehouseId;
    }
}
