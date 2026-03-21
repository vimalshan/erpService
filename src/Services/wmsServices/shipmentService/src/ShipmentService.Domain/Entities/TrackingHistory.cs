using ShipmentService.Domain.Common;

namespace ShipmentService.Domain.Entities;

public sealed class TrackingHistory : Entity
{
    private TrackingHistory() { }

    public int ShipmentId { get; private set; }
    public string Status { get; private set; } = default!;
    public string? Location { get; private set; }
    public string? Description { get; private set; }
    public DateTime EventDatetime { get; private set; }
    public string? CreatedBy { get; private set; }

    public Shipment Shipment { get; private set; } = default!;

    internal static TrackingHistory Create(int shipmentId, string status, string? location = null,
        string? description = null, string? createdBy = null)
    {
        if (string.IsNullOrWhiteSpace(status))
            throw new ArgumentException("Status is required.", nameof(status));

        return new TrackingHistory
        {
            ShipmentId = shipmentId,
            Status = status,
            Location = location,
            Description = description,
            EventDatetime = DateTime.UtcNow,
            CreatedBy = createdBy
        };
    }
}
