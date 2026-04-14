using WMTransactional.Domain.Common;

namespace WMTransactional.Domain.Entities;

public class ShipmentLine : BaseEntity
{
    public int ShipmentLineId { get; private set; }
    public int ShipmentId { get; private set; }
    public int SoLineId { get; private set; }
    public int ProductId { get; private set; }
    public int BinId { get; private set; }
    public decimal QuantityShipped { get; private set; }
    public string? LotNumber { get; private set; }
    public DateTime? ExpiryDate { get; private set; }
    public string? Notes { get; private set; }

    public Shipment Shipment { get; private set; } = null!;

    private ShipmentLine() { }

    public ShipmentLine(int soLineId, int productId, int binId, decimal quantityShipped, string? lotNumber, DateTime? expiryDate, string? notes)
    {
        SoLineId = soLineId;
        ProductId = productId;
        BinId = binId;
        QuantityShipped = quantityShipped;
        LotNumber = lotNumber;
        ExpiryDate = expiryDate;
        Notes = notes;
    }
}
