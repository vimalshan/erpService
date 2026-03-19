using PurchaseSalesService.Domain.Common;
using PurchaseSalesService.Domain.Entities;

namespace PurchaseSalesService.Domain.Events;

public sealed class SaleCreatedEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
    public SaleMain Sale { get; }

    public SaleCreatedEvent(SaleMain sale) => Sale = sale;
}

public sealed class SaleCancelledEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
    public SaleMain Sale { get; }
    public string CancelledBy { get; }

    public SaleCancelledEvent(SaleMain sale, string cancelledBy)
    {
        Sale = sale;
        CancelledBy = cancelledBy;
    }
}
