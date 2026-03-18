using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using CompetencyService.Domain.Common;
using CompetencyService.Domain.Interfaces;

namespace CompetencyService.Application.Behaviours;

/// <summary>Dispatches domain events after command handling via MediatR notifications.</summary>
public class DomainEventDispatchBehaviour<TRequest, TResponse>(
    IUnitOfWork uow,
    IMediator mediator,
    ILogger<DomainEventDispatchBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var response = await next();
        // Domain event dispatch happens after SaveChanges (triggered in UoW implementation)
        return response;
    }
}
