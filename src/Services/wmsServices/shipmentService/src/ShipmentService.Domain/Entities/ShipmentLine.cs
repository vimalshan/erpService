using ShipmentService.Domain.Common;

namespace ShipmentService.Domain.Entities;

public sealed class ShipmentLine : Entity
{
    private ShipmentLine() { }

    public int ShipmentId { get; private set; }
    public int? SoLineId { get; private set; }
    public int ProductId { get; private set; }
    public int BinId { get; private set; }
    public decimal QuantityShipped { get; private set; }
    public decimal? UnitPrice { get; private set; }
    public string? LotNumber { get; private set; }
    public DateOnly? ExpiryDate { get; private set; }
    public string? Notes { get; private set; }

    public Shipment Shipment { get; private set; } = default!;

    internal static ShipmentLine Create(int shipmentId, int productId, int binId, decimal quantityShipped,
        decimal? unitPrice = null, string? lotNumber = null, DateOnly? expiryDate = null,
        string? notes = null, int? soLineId = null)
    {
        if (quantityShipped <= 0)
            throw new ArgumentException("Quantity shipped must be greater than zero.", nameof(quantityShipped));

        return new ShipmentLine
        {
            ShipmentId = shipmentId,
            ProductId = productId,
            BinId = binId,
            QuantityShipped = quantityShipped,
            UnitPrice = unitPrice,
            LotNumber = lotNumber,
            ExpiryDate = expiryDate,
            Notes = notes,
            SoLineId = soLineId
        };
    }
}
