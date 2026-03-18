namespace FeedbackService.Domain.Common;

/// <summary>
/// Abstract base class for domain entities
/// </summary>
/// <summary>
/// Abstract base class for domain entities
/// </summary>
public abstract class AggregateRoot
{
    private readonly List<DomainEvent> _domainEvents;

    protected AggregateRoot()
    {
        _domainEvents = new List<DomainEvent>();
    }

    /// <summary>
    /// Gets the unique identifier of the entity
    /// </summary>
    public decimal Id { get; set; }

    /// <summary>
    /// Gets the creation timestamp
    /// </summary>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets the last update timestamp
    /// </summary>
    public DateTime? UpdatedOn { get; set; }

    /// <summary>
    /// Gets the domain events that should be published
    /// </summary>
    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Adds a domain event to be published
    /// </summary>
    protected void RaiseDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Clears all domain events
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
