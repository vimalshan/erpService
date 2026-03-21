using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EnergyService.Application.Common.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly Stopwatch _timer = new();
    private readonly ILogger<PerformanceBehaviour<TRequest, TResponse>> _logger;

    public PerformanceBehaviour(ILogger<PerformanceBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _timer.Start();
        var response = await next(cancellationToken);
        _timer.Stop();

        var elapsedMs = _timer.ElapsedMilliseconds;
        if (elapsedMs > 500)
        {
            _logger.LogWarning(
                "Energy Service Long Running Request: {Name} ({ElapsedMilliseconds}ms) {@Request}",
                typeof(TRequest).Name, elapsedMs, request);
        }

        return response;
    }
}
