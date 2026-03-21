using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ReceivingService.Application.Behaviours;

/// <summary>Logs a warning when a request exceeds a performance threshold (default 500 ms).</summary>
public sealed class PerformanceBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<PerformanceBehaviour<TRequest, TResponse>> _logger;
    private const int ThresholdMs = 500;

    public PerformanceBehaviour(ILogger<PerformanceBehaviour<TRequest, TResponse>> logger)
        => _logger = logger;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        var response = await next(cancellationToken);
        sw.Stop();

        if (sw.ElapsedMilliseconds > ThresholdMs)
            _logger.LogWarning(
                "Long running request: {Request} ({Elapsed} ms)",
                typeof(TRequest).Name, sw.ElapsedMilliseconds);

        return response;
    }
}
