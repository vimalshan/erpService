using VisitorServices.Domain.Common;

namespace VisitorServices.Domain.Events;

public sealed record VisitorCheckedOutEvent : IDomainEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
    public long VisitorId { get; init; }
    public DateTime CheckoutTime { get; init; }
    public long CheckedOutBy { get; init; }

    public VisitorCheckedOutEvent() { }

    public VisitorCheckedOutEvent(long visitorId, DateTime checkoutTime, long checkedOutBy)
    {
        VisitorId = visitorId;
        CheckoutTime = checkoutTime;
        CheckedOutBy = checkedOutBy;
    }
}
