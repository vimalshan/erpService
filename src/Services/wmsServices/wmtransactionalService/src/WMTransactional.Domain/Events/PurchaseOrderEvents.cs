using WMTransactional.Domain.Common;

namespace WMTransactional.Domain.Events;

public sealed record PurchaseOrderCreatedEvent(string PoNumber, int SupplierId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record PurchaseOrderConfirmedEvent(string PoNumber, int SupplierId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record PurchaseOrderCompletedEvent(string PoNumber, int SupplierId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}

public sealed record PurchaseOrderCancelledEvent(string PoNumber, int SupplierId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
