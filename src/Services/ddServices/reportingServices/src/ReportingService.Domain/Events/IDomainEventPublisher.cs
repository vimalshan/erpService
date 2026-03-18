namespace ReportingService.Domain.Events;

public interface IDomainEventPublisher
{
    Task PublishAsync(DomainEvent @event, CancellationToken cancellationToken = default);
    Task PublishAsync(IEnumerable<DomainEvent> events, CancellationToken cancellationToken = default);
}
