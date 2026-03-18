using MediatR;

namespace AuditService.Domain.Common;

public abstract class DomainEvent : INotification
{
    protected DomainEvent()
    {
        OccurredOn = DateTime.UtcNow;
        EventId = Guid.NewGuid();
    }

    public DateTime OccurredOn { get; }
    public Guid EventId { get; }
}
