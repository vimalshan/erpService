namespace ReferenceService.Domain;

/// <summary>
/// Base entity class for all entities in the domain.
/// Provides Id and audit tracking.
/// </summary>
public abstract class Entity<TId> where TId : notnull
{
    public TId Id { get; protected set; }
    
    public long LastModifiedBy { get; set; }
    
    public DateTime LastModifiedOn { get; set; }
    
    private List<DomainEvent> _domainEvents = [];
    
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    protected void AddDomainEvent(DomainEvent eventToAdd)
    {
        _domainEvents.Add(eventToAdd);
    }
    
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

public abstract record DomainEvent
{
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}
