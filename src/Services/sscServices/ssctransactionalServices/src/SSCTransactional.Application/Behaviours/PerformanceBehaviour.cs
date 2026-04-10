using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace SSCTransactional.Application.Behaviours;

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<PerformanceBehaviour<TRequest, TResponse>> _logger;

    public PerformanceBehaviour(ILogger<PerformanceBehaviour<TRequest, TResponse>> logger)
        => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        var response = await next(cancellationToken);
        sw.Stop();

        if (sw.ElapsedMilliseconds > 500)
            _logger.LogWarning("[Performance] {RequestName} took {ElapsedMs}ms. Request: {@Request}",
                typeof(TRequest).Name, sw.ElapsedMilliseconds, request);

        return response;
    }
}
