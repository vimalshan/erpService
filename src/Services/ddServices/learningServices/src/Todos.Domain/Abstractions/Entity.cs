namespace Todos.Domain.Abstractions;

/// <summary>
/// Base class for all entities
/// </summary>
public abstract class Entity
{
    /// <summary>
    /// Gets the entity's unique identifier
    /// </summary>
    public Guid Id { get; protected set; } = Guid.NewGuid();

    /// <summary>
    /// Gets the collection of domain events raised by this entity
    /// </summary>
    private readonly List<DomainEvent> _domainEvents = [];

    /// <summary>
    /// Gets a read-only collection of domain events
    /// </summary>
    public IReadOnlyList<DomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();

    /// <summary>
    /// Clears all domain events from this entity
    /// </summary>
    public void ClearDomainEvents() => _domainEvents.Clear();

    /// <summary>
    /// Raises a domain event
    /// </summary>
    protected void RaiseDomainEvent(DomainEvent domainEvent)
    {
        domainEvent.AggregateId = Id;
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current entity
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
            return false;

        var other = (Entity)obj;
        return Id == other.Id;
    }

    /// <summary>
    /// Serves as the default hash function
    /// </summary>
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    /// <summary>
    /// Determines whether two specified entities are equal
    /// </summary>
    public static bool operator ==(Entity left, Entity right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two specified entities are not equal
    /// </summary>
    public static bool operator !=(Entity left, Entity right)
    {
        return !left.Equals(right);
    }
}
