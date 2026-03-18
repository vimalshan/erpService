using InventoryManagement.Domain.Common;

namespace InventoryManagement.Domain.Events;

public sealed class ProductCreatedEvent : DomainEvent
{
    public int ProductId { get; }
    public string ProductName { get; }

    public ProductCreatedEvent(int productId, string productName)
    {
        ProductId = productId;
        ProductName = productName;
    }
}
