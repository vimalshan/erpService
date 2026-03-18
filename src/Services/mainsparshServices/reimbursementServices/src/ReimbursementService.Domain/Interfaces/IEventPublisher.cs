namespace ReimbursementService.Domain.Interfaces;

/// <summary>Abstraction for publishing domain events to a message broker.</summary>
public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class;
}
