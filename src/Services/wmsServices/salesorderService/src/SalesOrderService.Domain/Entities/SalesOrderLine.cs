using SalesOrderService.Domain.Common;
using SalesOrderService.Domain.Exceptions;

namespace SalesOrderService.Domain.Entities;

/// <summary>
/// SalesOrderLine entity — owned by the SalesOrder aggregate.
/// </summary>
public sealed class SalesOrderLine : BaseEntity
{
    // EF Core constructor
    private SalesOrderLine() { }

    public int SoLineId { get; private set; }
    public int SoId { get; private set; }
    public int ProductId { get; private set; }
    public int LineNumber { get; private set; }
    public decimal QuantityOrdered { get; private set; }
    public decimal QuantityShipped { get; private set; }
    public decimal? UnitPrice { get; private set; }
    public decimal Discount { get; private set; }
    public string? Notes { get; private set; }

    /// <summary>Line total: (unit_price - discount) * quantity_ordered</summary>
    public decimal LineTotal =>
        ((UnitPrice ?? 0m) - Discount) * QuantityOrdered;

    internal static SalesOrderLine Create(
        int soId, int productId, int lineNumber,
        decimal quantityOrdered, decimal? unitPrice,
        decimal discount, string? notes)
    {
        if (quantityOrdered <= 0)
            throw new SalesOrderDomainException("Quantity ordered must be greater than zero.");
        if (discount < 0)
            throw new SalesOrderDomainException("Discount cannot be negative.");

        return new SalesOrderLine
        {
            SoId            = soId,
            ProductId       = productId,
            LineNumber      = lineNumber,
            QuantityOrdered = quantityOrdered,
            QuantityShipped = 0,
            UnitPrice       = unitPrice,
            Discount        = discount,
            Notes           = notes
        };
    }

    public void RecordShipment(decimal quantityShipped)
    {
        if (quantityShipped < 0)
            throw new SalesOrderDomainException("Shipped quantity cannot be negative.");
        if (QuantityShipped + quantityShipped > QuantityOrdered)
            throw new SalesOrderDomainException("Shipped quantity exceeds ordered quantity.");
        QuantityShipped += quantityShipped;
    }

    public void UpdatePrice(decimal? unitPrice, decimal discount)
    {
        if (discount < 0) throw new SalesOrderDomainException("Discount cannot be negative.");
        UnitPrice = unitPrice;
        Discount  = discount;
    }
}
