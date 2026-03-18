namespace AuthProvider.Domain.Common;

/// <summary>
/// Base class for all domain aggregate roots.
/// Holds a collection of uncommitted domain events (Domain-Driven Design).
/// </summary>
public abstract class AggregateRoot
{
    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
}
