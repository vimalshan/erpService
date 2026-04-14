using WMTransactional.Domain.Common;

namespace WMTransactional.Domain.Entities;

public class ReceivingLine : BaseEntity
{
    public int ReceivingLineId { get; private set; }
    public int ReceivingId { get; private set; }
    public int PoLineId { get; private set; }
    public int ProductId { get; private set; }
    public int BinId { get; private set; }
    public decimal QuantityReceived { get; private set; }
    public string? LotNumber { get; private set; }
    public DateTime? ExpiryDate { get; private set; }
    public string? Notes { get; private set; }

    public Receiving Receiving { get; private set; } = null!;

    private ReceivingLine() { }

    public ReceivingLine(int poLineId, int productId, int binId, decimal quantityReceived, string? lotNumber, DateTime? expiryDate, string? notes)
    {
        PoLineId = poLineId;
        ProductId = productId;
        BinId = binId;
        QuantityReceived = quantityReceived;
        LotNumber = lotNumber;
        ExpiryDate = expiryDate;
        Notes = notes;
    }
}
