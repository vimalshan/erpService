namespace AuthProvider.Domain.Common;

/// <summary>Base class for all domain events.</summary>
public abstract class DomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
