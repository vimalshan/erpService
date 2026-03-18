using MediatR;
using SwipeTransactionService.Domain.Common;

namespace SwipeTransactionService.Infrastructure.Services;

public sealed class DomainEventDispatcher
{
    private readonly IMediator _mediator;

    public DomainEventDispatcher(IMediator mediator) => _mediator = mediator;

    public async Task DispatchAndClearAsync(IEnumerable<BaseEntity> entities, CancellationToken ct = default)
    {
        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        foreach (var entity in entities)
            entity.ClearDomainEvents();

        foreach (var domainEvent in domainEvents)
            await _mediator.Publish(domainEvent, ct);
    }
}
