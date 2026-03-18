using MasterDataService.Domain.Common;
using MediatR;

namespace MasterDataService.Application.Behaviours;

public class DomainEventBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IMediator _mediator;

    public DomainEventBehaviour(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();
        return response;
    }
}

public static class DomainEventDispatcher
{
    public static async Task DispatchEvents(IEnumerable<BaseEntity> entities, IMediator mediator, CancellationToken cancellationToken = default)
    {
        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();

        foreach (var entity in entities)
            entity.ClearDomainEvents();

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent, cancellationToken);
    }
}
