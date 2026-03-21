using MediatR;
using Microsoft.Extensions.Logging;
using TourPlanService.Application.Interfaces;
using TourPlanService.Domain.Common;

namespace TourPlanService.Application.Behaviours;

public sealed class TransactionBehaviour<TRequest, TResponse>(
    IUnitOfWork unitOfWork,
    ILogger<TransactionBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Only wrap commands (write operations) in transactions
        if (request.GetType().Name.EndsWith("Query"))
            return await next(cancellationToken);

        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var response = await next(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return response;
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}

public sealed class DomainEventDispatchBehaviour<TRequest, TResponse>(
    IMediator mediator,
    ILogger<DomainEventDispatchBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next(cancellationToken);
        return response;
    }
}
