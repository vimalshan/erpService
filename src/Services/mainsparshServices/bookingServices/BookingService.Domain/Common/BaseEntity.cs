namespace BookingService.Domain.Common;

public abstract class BaseEntity
{
    public long Id { get; protected set; }

    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();

    public long CreatedBy { get; protected set; }
    public DateTime CreatedOn { get; protected set; } = DateTime.UtcNow;
    public long? UpdatedBy { get; protected set; }
    public DateTime? UpdatedOn { get; protected set; }

    protected void SetUpdatedAudit(long updatedBy)
    {
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
