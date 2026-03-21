using PurchaseOrderService.Domain.Common;
using PurchaseOrderService.Domain.ValueObjects;

namespace PurchaseOrderService.Domain.Entities;

public class PurchaseOrderLine : Entity<int>
{
    public int PoId { get; private set; }
    public int ProductId { get; private set; }
    public int LineNumber { get; private set; }
    public decimal QuantityOrdered { get; private set; }
    public decimal QuantityReceived { get; private set; }
    public decimal? UnitPrice { get; private set; }
    public string? Notes { get; private set; }

    private PurchaseOrderLine() { } // EF constructor

    internal PurchaseOrderLine(int poId, int productId, int lineNumber, decimal quantityOrdered, decimal? unitPrice, string? notes)
    {
        if (quantityOrdered <= 0) throw new ArgumentException("Quantity ordered must be greater than zero.");

        PoId = poId;
        ProductId = productId;
        LineNumber = lineNumber;
        QuantityOrdered = quantityOrdered;
        QuantityReceived = 0;
        UnitPrice = unitPrice;
        Notes = notes;
    }

    public void Receive(decimal quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Receive quantity must be greater than zero.");
        if (QuantityReceived + quantity > QuantityOrdered)
            throw new InvalidOperationException("Cannot receive more than ordered quantity.");
        QuantityReceived += quantity;
    }

    public bool IsFullyReceived => QuantityReceived >= QuantityOrdered;

    public decimal? LineTotal => UnitPrice.HasValue ? UnitPrice.Value * QuantityOrdered : null;

    public void Update(decimal quantityOrdered, decimal? unitPrice, string? notes)
    {
        if (quantityOrdered <= 0) throw new ArgumentException("Quantity ordered must be greater than zero.");
        if (quantityOrdered < QuantityReceived)
            throw new InvalidOperationException("Cannot reduce ordered quantity below received quantity.");
        QuantityOrdered = quantityOrdered;
        UnitPrice = unitPrice;
        Notes = notes;
    }
}
