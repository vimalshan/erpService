using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace RecruitmentService.Application.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private const int SlowRequestThresholdMs = 500;
    private readonly ILogger<PerformanceBehaviour<TRequest, TResponse>> _logger;
    private readonly Stopwatch _timer = new();

    public PerformanceBehaviour(ILogger<PerformanceBehaviour<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        _timer.Restart();
        var response = await next(ct);
        _timer.Stop();

        var elapsed = _timer.ElapsedMilliseconds;
        if (elapsed > SlowRequestThresholdMs)
            _logger.LogWarning("[SLOW REQUEST] {RequestName} took {Elapsed}ms. Request: {@Request}",
                typeof(TRequest).Name, elapsed, request);

        return response;
    }
}
