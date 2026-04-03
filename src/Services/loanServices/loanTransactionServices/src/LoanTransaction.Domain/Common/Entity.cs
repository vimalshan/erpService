namespace LoanTransaction.Domain.Common;

public abstract class Entity
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
    public long CreatedBy { get; set; }
    public long ModifiedBy { get; set; }
    public bool IsDeleted { get; set; } = false;

    private readonly List<DomainEvent> _domainEvents = new();

    public IReadOnlyList<DomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();

    public void ClearDomainEvents() => _domainEvents.Clear();

    protected void RaiseDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}
