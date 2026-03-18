using MediatR;
using Shared.Core.Domain;

namespace Shared.Events;

/// <summary>
/// MediatR-based domain event publisher
/// Uses in-memory event publication for same-process handling
/// </summary>
public class MediatRDomainEventPublisher : IDomainEventPublisher
{
    private readonly IPublisher _mediator;

    public MediatRDomainEventPublisher(IPublisher mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default) where TEvent : DomainEvent
    {
        await _mediator.Publish(domainEvent, cancellationToken);
    }

    public async Task PublishMultipleAsync(IEnumerable<DomainEvent> domainEvents, CancellationToken cancellationToken = default)
    {
        var tasks = domainEvents.Select(e => 
            typeof(IPublisher)
                .GetMethod("Publish", new[] { e.GetType(), typeof(CancellationToken) })
                ?.Invoke(_mediator, new object[] { e, cancellationToken })
                as Task ?? Task.CompletedTask);

        await Task.WhenAll(tasks);
    }
}
