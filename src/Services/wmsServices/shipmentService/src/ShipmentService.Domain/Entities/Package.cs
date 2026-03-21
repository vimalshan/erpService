using ShipmentService.Domain.Common;

namespace ShipmentService.Domain.Entities;

public sealed class Package : Entity
{
    private Package() { }

    public int ShipmentId { get; private set; }
    public string PackageNumber { get; private set; } = default!;
    public decimal? Weight { get; private set; }
    public decimal? Volume { get; private set; }
    public string? Dimensions { get; private set; }
    public string? TrackingNumber { get; private set; }
    public string? ContentsDescription { get; private set; }

    public Shipment Shipment { get; private set; } = default!;

    internal static Package Create(int shipmentId, string packageNumber, decimal? weight = null,
        decimal? volume = null, string? dimensions = null, string? trackingNumber = null,
        string? contentsDescription = null)
    {
        if (string.IsNullOrWhiteSpace(packageNumber))
            throw new ArgumentException("Package number is required.", nameof(packageNumber));

        return new Package
        {
            ShipmentId = shipmentId,
            PackageNumber = packageNumber.Trim(),
            Weight = weight,
            Volume = volume,
            Dimensions = dimensions,
            TrackingNumber = trackingNumber,
            ContentsDescription = contentsDescription
        };
    }
}
