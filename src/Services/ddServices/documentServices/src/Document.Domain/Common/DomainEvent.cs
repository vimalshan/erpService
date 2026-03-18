using MediatR;

namespace Document.Domain.Common;

public abstract class DomainEvent : INotification
{
    protected DomainEvent()
    {
        OccurredOn = DateTime.UtcNow;
    }

    public DateTime OccurredOn { get; }
}
