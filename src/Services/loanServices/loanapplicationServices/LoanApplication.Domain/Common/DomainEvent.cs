using MediatR;

namespace LoanApplication.Domain.Common;

/// <summary>
/// Base class for all domain events
/// </summary>
public abstract class DomainEvent : INotification
{
    public DomainEvent()
    {
        Id = Guid.NewGuid();
        OccurredAt = DateTime.UtcNow;
    }

    public Guid Id { get; }
    public DateTime OccurredAt { get; }
}
