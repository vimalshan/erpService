using ProductService.Domain.Common;
using ProductService.Domain.Entities;

namespace ProductService.Domain.Events;

public sealed class ProductUpdatedEvent : IDomainEvent
{
    public Product Product { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public ProductUpdatedEvent(Product product) => Product = product;
}
