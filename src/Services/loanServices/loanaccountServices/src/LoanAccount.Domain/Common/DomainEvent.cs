namespace LoanAccount.Domain.Common;

/// <summary>
/// Base class for domain events
/// </summary>
public abstract class DomainEvent
{
    public long AggregateId { get; protected set; }
    public DateTime OccurredAt { get; protected set; } = DateTime.UtcNow;
}
