using WMTransactional.Domain.Common;

namespace WMTransactional.Domain.Entities;

public class PurchaseOrderLine : BaseEntity
{
    public int PoLineId { get; private set; }
    public int PoId { get; private set; }
    public int ProductId { get; private set; }
    public int LineNumber { get; private set; }
    public decimal QuantityOrdered { get; private set; }
    public decimal QuantityReceived { get; private set; }
    public decimal? UnitPrice { get; private set; }
    public string? Notes { get; private set; }

    public PurchaseOrder PurchaseOrder { get; private set; } = null!;

    private PurchaseOrderLine() { }

    public PurchaseOrderLine(int productId, int lineNumber, decimal quantityOrdered, decimal? unitPrice, string? notes)
    {
        ProductId = productId;
        LineNumber = lineNumber;
        QuantityOrdered = quantityOrdered;
        QuantityReceived = 0;
        UnitPrice = unitPrice;
        Notes = notes;
    }

    public void ReceiveQuantity(decimal quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Receive quantity must be positive.");

        QuantityReceived += quantity;
    }

    public decimal RemainingQuantity => QuantityOrdered - QuantityReceived;
    public bool IsFullyReceived => QuantityReceived >= QuantityOrdered;
}
