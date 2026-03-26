using MediatR;
using CanteenTransactionService.Domain.Common;

namespace CanteenTransactionService.Infrastructure.DomainEvents;

public class DomainEventDispatcher
{
    private readonly IMediator _mediator;

    public DomainEventDispatcher(IMediator mediator) => _mediator = mediator;

    public async Task DispatchEventsAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken ct = default)
    {
        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent, ct);
        }
    }
}
