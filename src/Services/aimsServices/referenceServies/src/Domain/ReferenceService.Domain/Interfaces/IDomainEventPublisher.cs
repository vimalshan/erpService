namespace ReferenceService.Domain.Interfaces;

/// <summary>
/// Interface for publishing domain events.
/// </summary>
public interface IDomainEventPublisher
{
    Task PublishAsync(DomainEvent domainEvent, CancellationToken cancellationToken = default);
    Task PublishMultipleAsync(IEnumerable<DomainEvent> domainEvents, CancellationToken cancellationToken = default);
}
