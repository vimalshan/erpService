using ShipmentService.Domain.Common;
using ShipmentService.Domain.Enums;

namespace ShipmentService.Domain.Events;

public sealed class ShipmentStatusChangedEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public string ShipmentNumber { get; }
    public ShipmentStatus PreviousStatus { get; }
    public ShipmentStatus NewStatus { get; }
    public string? Location { get; }
    public string? UpdatedBy { get; }

    public ShipmentStatusChangedEvent(string shipmentNumber, ShipmentStatus previousStatus,
        ShipmentStatus newStatus, string? location, string? updatedBy)
    {
        ShipmentNumber = shipmentNumber;
        PreviousStatus = previousStatus;
        NewStatus = newStatus;
        Location = location;
        UpdatedBy = updatedBy;
    }
}
