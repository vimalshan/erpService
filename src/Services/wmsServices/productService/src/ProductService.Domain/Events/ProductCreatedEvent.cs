using ProductService.Domain.Common;
using ProductService.Domain.Entities;

namespace ProductService.Domain.Events;

public sealed class ProductCreatedEvent : IDomainEvent
{
    public Product Product { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public ProductCreatedEvent(Product product) => Product = product;
}
