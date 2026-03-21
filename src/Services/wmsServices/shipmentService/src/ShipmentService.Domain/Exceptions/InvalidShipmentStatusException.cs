using ShipmentService.Domain.Enums;

namespace ShipmentService.Domain.Exceptions;

public sealed class InvalidShipmentStatusException : Exception
{
    public InvalidShipmentStatusException(ShipmentStatus current, ShipmentStatus attempted)
        : base($"Cannot transition shipment from '{current}' to '{attempted}'.") { }
}
