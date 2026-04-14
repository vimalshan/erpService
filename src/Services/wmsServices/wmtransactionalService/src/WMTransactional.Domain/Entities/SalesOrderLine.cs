using WMTransactional.Domain.Common;

namespace WMTransactional.Domain.Entities;

public class SalesOrderLine : BaseEntity
{
    public int SoLineId { get; private set; }
    public int SoId { get; private set; }
    public int ProductId { get; private set; }
    public int LineNumber { get; private set; }
    public decimal QuantityOrdered { get; private set; }
    public decimal QuantityShipped { get; private set; }
    public decimal? UnitPrice { get; private set; }
    public string? Notes { get; private set; }

    public SalesOrder SalesOrder { get; private set; } = null!;

    private SalesOrderLine() { }

    public SalesOrderLine(int productId, int lineNumber, decimal quantityOrdered, decimal? unitPrice, string? notes)
    {
        ProductId = productId;
        LineNumber = lineNumber;
        QuantityOrdered = quantityOrdered;
        QuantityShipped = 0;
        UnitPrice = unitPrice;
        Notes = notes;
    }

    public void ShipQuantity(decimal quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Ship quantity must be positive.");

        QuantityShipped += quantity;
    }

    public decimal RemainingQuantity => QuantityOrdered - QuantityShipped;
    public bool IsFullyShipped => QuantityShipped >= QuantityOrdered;
}
