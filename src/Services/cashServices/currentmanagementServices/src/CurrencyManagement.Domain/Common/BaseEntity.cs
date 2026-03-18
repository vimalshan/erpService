namespace CurrencyManagement.Domain.Common;

/// <summary>
/// Base class for all domain entities supporting domain events
/// </summary>
public abstract class BaseEntity
{
    private readonly List<DomainEvent> _domainEvents = new();

    /// <summary>
    /// Gets the collection of domain events that occur within this entity
    /// </summary>
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Adds a domain event to be published
    /// </summary>
    protected void RaiseDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Clears all domain events (typically called after publishing)
    /// </summary>
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
