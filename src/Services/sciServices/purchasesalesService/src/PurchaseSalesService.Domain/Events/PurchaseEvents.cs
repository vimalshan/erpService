using PurchaseSalesService.Domain.Common;
using PurchaseSalesService.Domain.Entities;

namespace PurchaseSalesService.Domain.Events;

public sealed class PurchaseCreatedEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
    public PurchaseDetail Purchase { get; }

    public PurchaseCreatedEvent(PurchaseDetail purchase) => Purchase = purchase;
}

public sealed class PurchaseCancelledEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
    public PurchaseDetail Purchase { get; }
    public string CancelledBy { get; }

    public PurchaseCancelledEvent(PurchaseDetail purchase, string cancelledBy)
    {
        Purchase = purchase;
        CancelledBy = cancelledBy;
    }
}
