namespace Shared.Core.Domain;

/// <summary>
/// Aggregate Root in DDD - Entities are accessed through the aggregate root
/// Aggregates form the boundary of consistency in DDD
/// </summary>
public abstract class AggregateRoot<TId> : Entity<TId> where TId : notnull
{
    protected AggregateRoot() { }

    protected AggregateRoot(TId id) : base(id) { }

    /// <summary>
    /// Gets the aggregate's version for optimistic concurrency control
    /// </summary>
    public int Version { get; protected set; } = 1;

    /// <summary>
    /// Check if aggregate is valid before persistence
    /// </summary>
    public virtual void ValidateInvariants()
    {
        // Override in derived classes to implement invariants
    }
}
