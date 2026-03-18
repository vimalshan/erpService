using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BatchService.Application.Behaviours;

/// <summary>Warns when a request takes longer than the configured threshold.</summary>
public sealed class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private const int WarningThresholdMs = 500;
    private readonly ILogger<PerformanceBehaviour<TRequest, TResponse>> _logger;

    public PerformanceBehaviour(ILogger<PerformanceBehaviour<TRequest, TResponse>> logger)
        => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var sw = Stopwatch.StartNew();
        var response = await next(ct);
        sw.Stop();

        if (sw.ElapsedMilliseconds > WarningThresholdMs)
            _logger.LogWarning("[PERF] Slow request: {Request} took {Elapsed} ms", typeof(TRequest).Name, sw.ElapsedMilliseconds);

        return response;
    }
}
