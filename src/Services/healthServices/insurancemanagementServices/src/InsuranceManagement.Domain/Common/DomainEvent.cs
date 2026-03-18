namespace InsuranceManagement.Domain.Common;

/// <summary>
/// Base class for domain events
/// </summary>
public abstract class DomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public string AggregateName => GetType().Name;
}
