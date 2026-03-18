namespace LoanAccount.Domain.Common;

/// <summary>
/// Base class for all entities in the domain
/// </summary>
public abstract class Entity
{
    protected Entity() { }
    protected Entity(long id) => Id = id;

    public long Id { get; protected set; }
    public DateTime CreatedOn { get; internal set; } = DateTime.UtcNow;
    public long CreatedBy { get; internal set; }
    public DateTime ModifiedOn { get; internal set; } = DateTime.UtcNow;
    public long ModifiedBy { get; internal set; }

    private readonly List<DomainEvent> _domainEvents = [];

    public IReadOnlyList<DomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();
    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void RaiseDomainEvent(DomainEvent @event) => _domainEvents.Add(@event);

    public override bool Equals(object? obj)
    {
        if (obj is not Entity entity) return false;
        return entity.Id == Id && entity.GetType() == GetType();
    }

    public override int GetHashCode() => Id.GetHashCode() * 41;
}
