using ShipmentService.Domain.Common;

namespace ShipmentService.Domain.Events;

public sealed class ShipmentDeliveredEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public string ShipmentNumber { get; }
    public int CustomerId { get; }
    public DateTime DeliveredAt { get; }

    public ShipmentDeliveredEvent(string shipmentNumber, int customerId, DateTime deliveredAt)
    {
        ShipmentNumber = shipmentNumber;
        CustomerId = customerId;
        DeliveredAt = deliveredAt;
    }
}
