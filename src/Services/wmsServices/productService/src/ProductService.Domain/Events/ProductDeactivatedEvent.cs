using ProductService.Domain.Common;

namespace ProductService.Domain.Events;

public sealed class ProductDeactivatedEvent : IDomainEvent
{
    public int ProductId { get; }
    public string Sku { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public ProductDeactivatedEvent(int productId, string sku)
    {
        ProductId = productId;
        Sku = sku;
    }
}
