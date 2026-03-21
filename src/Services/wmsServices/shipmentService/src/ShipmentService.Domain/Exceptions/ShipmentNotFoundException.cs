namespace ShipmentService.Domain.Exceptions;

public sealed class ShipmentNotFoundException : Exception
{
    public ShipmentNotFoundException(int shipmentId)
        : base($"Shipment with ID {shipmentId} was not found.") { }

    public ShipmentNotFoundException(string shipmentNumber)
        : base($"Shipment with number '{shipmentNumber}' was not found.") { }
}
