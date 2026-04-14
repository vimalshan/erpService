using WMTransactional.Domain.Common;
using WMTransactional.Domain.Events;

namespace WMTransactional.Domain.Entities;

public class Shipment : BaseEntity
{
    public int ShipmentId { get; private set; }
    public string ShipmentNumber { get; private set; } = null!;
    public int SoId { get; private set; }
    public DateTime ShippedDate { get; private set; }
    public string Status { get; private set; } = null!;
    public string? TrackingNumber { get; private set; }
    public string? Carrier { get; private set; }
    public string? Notes { get; private set; }
    public string? CreatedBy { get; private set; }
    public DateTime CreatedDate { get; private set; }

    private readonly List<ShipmentLine> _lines = [];
    public IReadOnlyCollection<ShipmentLine> Lines => _lines.AsReadOnly();

    public SalesOrder SalesOrder { get; private set; } = null!;

    private Shipment() { }

    public Shipment(string shipmentNumber, int soId, string? trackingNumber, string? carrier, string? notes, string? createdBy)
    {
        ShipmentNumber = shipmentNumber;
        SoId = soId;
        ShippedDate = DateTime.UtcNow;
        Status = "OPEN";
        TrackingNumber = trackingNumber;
        Carrier = carrier;
        Notes = notes;
        CreatedBy = createdBy;
        CreatedDate = DateTime.UtcNow;

        AddDomainEvent(new ShipmentCreatedEvent(shipmentNumber, soId));
    }

    public void AddLine(int soLineId, int productId, int binId, decimal quantityShipped, string? lotNumber, DateTime? expiryDate, string? notes)
    {
        if (Status != "OPEN")
            throw new InvalidOperationException("Cannot add lines to a non-open shipment.");

        var line = new ShipmentLine(soLineId, productId, binId, quantityShipped, lotNumber, expiryDate, notes);
        _lines.Add(line);
    }

    public void Ship()
    {
        if (Status != "OPEN")
            throw new InvalidOperationException("Only open shipments can be shipped.");
        if (_lines.Count == 0)
            throw new InvalidOperationException("Cannot ship a shipment with no lines.");

        Status = "SHIPPED";
        ShippedDate = DateTime.UtcNow;
        AddDomainEvent(new ShipmentShippedEvent(ShipmentNumber, SoId));
    }

    public void Cancel()
    {
        if (Status == "SHIPPED" || Status == "CANCELLED")
            throw new InvalidOperationException($"Cannot cancel a {Status} shipment.");

        Status = "CANCELLED";
        AddDomainEvent(new ShipmentCancelledEvent(ShipmentNumber, SoId));
    }
}
