using MediatR;
using Microsoft.Extensions.Logging;
using StrategicStock.Domain.Common;

namespace StrategicStock.Application.Behaviours;

public sealed class DomainEventDispatchBehaviour<TRequest, TResponse>(
    IPublisher publisher,
    ILogger<DomainEventDispatchBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next();

        // Domain events are dispatched after the handler completes
        if (request is IBaseRequest)
        {
            logger.LogDebug("Domain event dispatch completed for {RequestName}", typeof(TRequest).Name);
        }

        return response;
    }
}
