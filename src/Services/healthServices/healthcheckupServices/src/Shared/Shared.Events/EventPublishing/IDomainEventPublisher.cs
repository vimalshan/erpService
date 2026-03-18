using Shared.Core.Domain;

namespace Shared.Events;

/// <summary>
/// Domain event publisher for event-driven architecture
/// Publishes domain events to all subscribers
/// </summary>
public interface IDomainEventPublisher
{
    Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default) where TEvent : DomainEvent;
    Task PublishMultipleAsync(IEnumerable<DomainEvent> domainEvents, CancellationToken cancellationToken = default);
}

/// <summary>
/// Handler interface for domain events
/// Implement this to handle specific domain event types
/// </summary>
public interface IDomainEventHandler<in TEvent> where TEvent : DomainEvent
{
    Task Handle(TEvent domainEvent, CancellationToken cancellationToken = default);
}

/// <summary>
/// Integration event publisher for asynchronous communication between services
/// </summary>
public interface IIntegrationEventPublisher
{
    Task PublishAsync<TEvent>(TEvent integrationEvent, CancellationToken cancellationToken = default) where TEvent : IntegrationEvent;
}

/// <summary>
/// Handler for integration events
/// </summary>
public interface IIntegrationEventHandler<in TEvent> where TEvent : IntegrationEvent
{
    Task Handle(TEvent integrationEvent, CancellationToken cancellationToken = default);
}
