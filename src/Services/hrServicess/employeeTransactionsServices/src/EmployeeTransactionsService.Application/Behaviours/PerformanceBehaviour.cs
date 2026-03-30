using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmployeeTransactionsService.Application.Behaviours;

public sealed class PerformanceBehaviour<TRequest, TResponse>(ILogger<PerformanceBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private const int SlowRequestThresholdMs = 500;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        var response = await next();
        stopwatch.Stop();

        if (stopwatch.ElapsedMilliseconds > SlowRequestThresholdMs)
        {
            logger.LogWarning("Slow request detected: {RequestName} took {ElapsedMs}ms", typeof(TRequest).Name, stopwatch.ElapsedMilliseconds);
        }

        return response;
    }
}