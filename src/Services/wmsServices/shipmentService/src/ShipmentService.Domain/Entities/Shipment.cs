using ShipmentService.Domain.Common;
using ShipmentService.Domain.Enums;
using ShipmentService.Domain.Events;
using ShipmentService.Domain.Exceptions;

namespace ShipmentService.Domain.Entities;

/// <summary>Shipment aggregate root — owns ShipmentLines, Packages, TrackingHistory, DeliveryAttempts.</summary>
public sealed class Shipment : AggregateRoot
{
    private readonly List<ShipmentLine> _lines = [];
    private readonly List<Package> _packages = [];
    private readonly List<TrackingHistory> _trackingHistory = [];
    private readonly List<DeliveryAttempt> _deliveryAttempts = [];

    // EF Core private constructor
    private Shipment() { }

    public string ShipmentNumber { get; private set; } = default!;
    public int? SoId { get; private set; }
    public int CustomerId { get; private set; }
    public int WarehouseId { get; private set; }
    public ShipmentType ShipmentType { get; private set; }
    public string? ServiceType { get; private set; }
    public DateTime ShippedDate { get; private set; }
    public ShipmentStatus Status { get; private set; }
    public string? TrackingNumber { get; private set; }
    public string? Carrier { get; private set; }
    public decimal? TotalWeight { get; private set; }
    public decimal? TotalVolume { get; private set; }
    public string? SpecialInstructions { get; private set; }
    public string? Notes { get; private set; }
    public string? CreatedBy { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime ModifiedDate { get; private set; }

    public IReadOnlyCollection<ShipmentLine> Lines => _lines.AsReadOnly();
    public IReadOnlyCollection<Package> Packages => _packages.AsReadOnly();
    public IReadOnlyCollection<TrackingHistory> TrackingHistory => _trackingHistory.AsReadOnly();
    public IReadOnlyCollection<DeliveryAttempt> DeliveryAttempts => _deliveryAttempts.AsReadOnly();

    public static Shipment Create(
        string shipmentNumber,
        int customerId,
        int warehouseId,
        ShipmentType shipmentType,
        string? serviceType = null,
        string? carrier = null,
        string? trackingNumber = null,
        string? specialInstructions = null,
        string? createdBy = null,
        int? soId = null)
    {
        if (string.IsNullOrWhiteSpace(shipmentNumber))
            throw new ArgumentException("Shipment number is required.", nameof(shipmentNumber));

        var shipment = new Shipment
        {
            ShipmentNumber = shipmentNumber.Trim().ToUpper(),
            CustomerId = customerId,
            WarehouseId = warehouseId,
            ShipmentType = shipmentType,
            ServiceType = serviceType,
            Carrier = carrier,
            TrackingNumber = trackingNumber,
            SpecialInstructions = specialInstructions,
            CreatedBy = createdBy,
            SoId = soId,
            Status = ShipmentStatus.Pending,
            ShippedDate = DateTime.UtcNow,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        shipment.AddDomainEvent(new ShipmentCreatedEvent(shipment.ShipmentNumber, customerId, warehouseId));
        return shipment;
    }

    public void UpdateStatus(ShipmentStatus newStatus, string? location = null, string? description = null, string? updatedBy = null)
    {
        if (!IsValidTransition(Status, newStatus))
            throw new InvalidShipmentStatusException(Status, newStatus);

        var previousStatus = Status;
        Status = newStatus;
        ModifiedDate = DateTime.UtcNow;

        // Use global qualifier to avoid ambiguity with the TrackingHistory property
        var entry = global::ShipmentService.Domain.Entities.TrackingHistory.Create(Id, newStatus.ToString(), location, description, updatedBy);
        _trackingHistory.Add(entry);
        AddDomainEvent(new ShipmentStatusChangedEvent(ShipmentNumber, previousStatus, newStatus, location, updatedBy));

        if (newStatus == ShipmentStatus.Delivered)
            AddDomainEvent(new ShipmentDeliveredEvent(ShipmentNumber, CustomerId, DateTime.UtcNow));
    }

    public ShipmentLine AddLine(int productId, int binId, decimal quantityShipped, decimal? unitPrice = null,
        string? lotNumber = null, DateOnly? expiryDate = null, string? notes = null, int? soLineId = null)
    {
        if (Status != ShipmentStatus.Pending && Status != ShipmentStatus.Open)
            throw new InvalidOperationException("Lines can only be added to PENDING or OPEN shipments.");

        var line = ShipmentLine.Create(Id, productId, binId, quantityShipped, unitPrice, lotNumber, expiryDate, notes, soLineId);
        _lines.Add(line);
        RecalculateTotals();
        return line;
    }

    public Package AddPackage(string packageNumber, decimal? weight = null, decimal? volume = null,
        string? dimensions = null, string? packageTrackingNumber = null, string? contentsDescription = null)
    {
        if (_packages.Any(p => p.PackageNumber == packageNumber))
            throw new InvalidOperationException($"Package {packageNumber} already exists in this shipment.");

        var package = Package.Create(Id, packageNumber, weight, volume, dimensions, packageTrackingNumber, contentsDescription);
        _packages.Add(package);
        return package;
    }

    public DeliveryAttempt RecordDeliveryAttempt(DateTime attemptDate, DeliveryResult result,
        string? reason = null, string? notes = null)
    {
        var attempt = DeliveryAttempt.Create(Id, attemptDate, result, reason, notes);
        _deliveryAttempts.Add(attempt);
        return attempt;
    }

    public void UpdateTrackingNumber(string trackingNumber)
    {
        TrackingNumber = trackingNumber;
        ModifiedDate = DateTime.UtcNow;
    }

    private void RecalculateTotals()
    {
        TotalWeight = _lines.Sum(l => l.QuantityShipped);
        ModifiedDate = DateTime.UtcNow;
    }

    private static bool IsValidTransition(ShipmentStatus current, ShipmentStatus next) => (current, next) switch
    {
        (ShipmentStatus.Pending, ShipmentStatus.Open) => true,
        (ShipmentStatus.Pending, ShipmentStatus.Cancelled) => true,
        (ShipmentStatus.Open, ShipmentStatus.PickedUp) => true,
        (ShipmentStatus.Open, ShipmentStatus.Cancelled) => true,
        (ShipmentStatus.PickedUp, ShipmentStatus.InTransit) => true,
        (ShipmentStatus.InTransit, ShipmentStatus.Shipped) => true,
        (ShipmentStatus.InTransit, ShipmentStatus.Exception) => true,
        (ShipmentStatus.Shipped, ShipmentStatus.Delivered) => true,
        (ShipmentStatus.Shipped, ShipmentStatus.Exception) => true,
        (ShipmentStatus.Exception, ShipmentStatus.InTransit) => true,
        (ShipmentStatus.Exception, ShipmentStatus.Cancelled) => true,
        _ => false
    };
}
