namespace EmailNotification.Domain.Common;

/// <summary>
/// Base class for all entities in the domain
/// </summary>
public abstract class Entity
{
    /// <summary>
    /// Entity ID
    /// </summary>
    public long Id { get; protected set; }

    /// <summary>
    /// Indicates if the entity is deleted
    /// </summary>
    public bool IsDeleted { get; protected set; }

    /// <summary>
    /// Date and time when the entity was created
    /// </summary>
    public DateTime CreatedAt { get; protected set; }

    /// <summary>
    /// Date and time when the entity was last modified
    /// </summary>
    public DateTime ModifiedAt { get; protected set; }

    /// <summary>
    /// User ID who created the entity
    /// </summary>
    public long CreatedBy { get; protected set; }

    /// <summary>
    /// User ID who last modified the entity
    /// </summary>
    public long ModifiedBy { get; protected set; }

    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// Gets the collection of domain events raised by this entity
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Gets the list of domain events raised by this entity
    /// </summary>
    /// <returns>Read-only collection of domain events</returns>
    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();

    /// <summary>
    /// Adds a domain event to the entity
    /// </summary>
    /// <param name="domainEvent">The domain event to add</param>
    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Clears all domain events from the entity
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
