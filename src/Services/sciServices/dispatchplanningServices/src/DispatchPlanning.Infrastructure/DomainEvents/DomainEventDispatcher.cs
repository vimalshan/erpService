using DispatchPlanning.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DispatchPlanning.Infrastructure.DomainEvents;

public class DomainEventDispatcher
{
    private readonly IMediator _mediator;
    private readonly ILogger<DomainEventDispatcher> _logger;

    public DomainEventDispatcher(IMediator mediator, ILogger<DomainEventDispatcher> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken ct = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            _logger.LogInformation("Dispatching domain event {EventType} [{EventId}]",
                domainEvent.GetType().Name, domainEvent.EventId);
            await _mediator.Publish(domainEvent, ct);
        }
    }
}
