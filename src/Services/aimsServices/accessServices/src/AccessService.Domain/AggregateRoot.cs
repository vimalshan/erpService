namespace AccessService.Domain;

/// <summary>
/// Base aggregate root class for domain aggregates
/// </summary>
public abstract class AggregateRoot : Entity
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent eventToAdd)
    {
        _domainEvents.Add(eventToAdd);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
