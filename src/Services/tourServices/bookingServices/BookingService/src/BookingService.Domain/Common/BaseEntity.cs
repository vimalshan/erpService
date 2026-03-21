using MediatR;

namespace BookingService.Domain.Common;

public abstract class BaseEntity
{
    private readonly List<INotification> _domainEvents = [];

    public DateTime LastModifiedOn { get; set; } = DateTime.UtcNow;

    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(INotification domainEvent) => _domainEvents.Add(domainEvent);
    public void RemoveDomainEvent(INotification domainEvent) => _domainEvents.Remove(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
}
