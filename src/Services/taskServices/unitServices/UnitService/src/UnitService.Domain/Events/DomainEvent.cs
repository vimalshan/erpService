using MediatR;

namespace UnitService.Domain.Events;

public abstract record DomainEvent : INotification
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
