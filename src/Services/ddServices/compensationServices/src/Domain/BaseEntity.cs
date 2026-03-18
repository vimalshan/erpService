namespace CompensationService.Domain;

/// <summary>
/// Base class for all entities in the compensation service domain.
/// </summary>
public abstract class BaseEntity
{
    /// <summary>Gets or sets the unique identifier.</summary>
    public decimal Id { get; set; }

    /// <summary>Gets the domain events that have occurred in this entity.</summary>
    private readonly List<DomainEvent> _domainEvents = new();

    /// <summary>Gets the domain events.</summary>
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>Adds a domain event to the entity.</summary>
    protected void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>Clears all domain events from the entity.</summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

/// <summary>
/// Base class for all domain events.
/// </summary>
public abstract class DomainEvent
{
    /// <summary>Gets the timestamp when the event occurred.</summary>
    public DateTime OccurredAt { get; } = DateTime.UtcNow;

    /// <summary>Gets the unique event identifier.</summary>
    public string EventId { get; } = Guid.NewGuid().ToString();
}
