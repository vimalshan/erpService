using System;
using System.Collections.Generic;
using MediatR;

namespace AppraisalService.Domain;

/// <summary>
/// Base entity with common auditing fields
/// </summary>
public abstract class Entity
{
    public long Id { get; protected set; }
    public DateTime CreatedOn { get; protected set; }
    public DateTime ModifiedOn { get; protected set; }

    protected Entity()
    {
    }

    protected Entity(long id)
    {
        Id = id;
        CreatedOn = DateTime.UtcNow;
        ModifiedOn = DateTime.UtcNow;
    }
}

/// <summary>
/// Base aggregate root with domain events
/// </summary>
public abstract class AggregateRoot : Entity
{
    private readonly List<DomainEvent> _domainEvents = new();

    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected AggregateRoot()
    {
    }

    protected AggregateRoot(long id) : base(id)
    {
    }

    protected void RaiseDomainEvent(DomainEvent @event)
    {
        _domainEvents.Add(@event);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

/// <summary>
/// Base domain event
/// </summary>
public abstract class DomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
    public Guid EventId { get; } = Guid.NewGuid();
}
