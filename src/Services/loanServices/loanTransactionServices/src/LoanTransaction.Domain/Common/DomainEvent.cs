using MediatR;

namespace LoanTransaction.Domain.Common;

public abstract class DomainEvent : INotification
{
    protected DomainEvent()
    {
        EventId = Guid.NewGuid();
        OccurredAt = DateTime.UtcNow;
    }

    public Guid EventId { get; }
    public DateTime OccurredAt { get; }
}
