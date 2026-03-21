using ShipmentService.Domain.Common;
using ShipmentService.Domain.Enums;

namespace ShipmentService.Domain.Entities;

public sealed class DeliveryAttempt : Entity
{
    private DeliveryAttempt() { }

    public int ShipmentId { get; private set; }
    public DateTime AttemptDate { get; private set; }
    public DeliveryResult Result { get; private set; }
    public string? Reason { get; private set; }
    public string? Notes { get; private set; }

    public Shipment Shipment { get; private set; } = default!;

    internal static DeliveryAttempt Create(int shipmentId, DateTime attemptDate, DeliveryResult result,
        string? reason = null, string? notes = null)
    {
        return new DeliveryAttempt
        {
            ShipmentId = shipmentId,
            AttemptDate = attemptDate,
            Result = result,
            Reason = reason,
            Notes = notes
        };
    }
}
