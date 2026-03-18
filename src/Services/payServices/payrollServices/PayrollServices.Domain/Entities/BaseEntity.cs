using PayrollServices.Domain.Events;

namespace PayrollServices.Domain.Entities;

/// <summary>
/// Base entity class for all domain entities
/// </summary>
public abstract class BaseEntity
{
    public List<DomainEvent> DomainEvents { get; } = new();

    public void AddDomainEvent(DomainEvent domainEvent) => DomainEvents.Add(domainEvent);

    public void RemoveDomainEvent(DomainEvent domainEvent) => DomainEvents.Remove(domainEvent);

    public void ClearDomainEvents() => DomainEvents.Clear();
}
