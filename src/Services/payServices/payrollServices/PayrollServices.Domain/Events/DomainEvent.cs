using MediatR;

namespace PayrollServices.Domain.Events;

/// <summary>
/// Base class for domain events
/// </summary>
public abstract record DomainEvent : INotification
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
