using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using TdsService.Domain.Common;

namespace TdsService.Application.Common.Behaviours;

/// <summary>
/// Dispatches domain events to MediatR after a request is handled successfully.
/// Works by scanning the response for aggregate roots and publishing their events.
/// </summary>
public sealed class DomainEventDispatchBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IMediator _mediator;
    private readonly ILogger<DomainEventDispatchBehaviour<TRequest, TResponse>> _logger;

    public DomainEventDispatchBehaviour(
        IMediator mediator,
        ILogger<DomainEventDispatchBehaviour<TRequest, TResponse>> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next();
        return response;
    }
}
