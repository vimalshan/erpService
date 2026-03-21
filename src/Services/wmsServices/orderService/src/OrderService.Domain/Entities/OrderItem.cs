using OrderService.Domain.Common;

namespace OrderService.Domain.Entities;

public class OrderItem : Entity
{
    public int OrderItemId { get; private set; }
    public int OrderId { get; private set; }
    public int ProductId { get; private set; }
    public int Quantity { get; private set; }
    public decimal UnitPrice { get; private set; }
    public decimal Discount { get; private set; }
    public string? Notes { get; private set; }

    private OrderItem() { } // EF constructor

    public OrderItem(int productId, int quantity, decimal unitPrice, decimal discount = 0, string? notes = null)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        if (unitPrice < 0) throw new ArgumentException("Unit price cannot be negative.", nameof(unitPrice));

        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        Discount = discount;
        Notes = notes;
    }

    public decimal LineTotal => (UnitPrice * Quantity) - Discount;

    public void UpdateQuantity(int quantity)
    {
        if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));
        Quantity = quantity;
    }

    public void UpdateDiscount(decimal discount)
    {
        if (discount < 0) throw new ArgumentException("Discount cannot be negative.", nameof(discount));
        Discount = discount;
    }
}
