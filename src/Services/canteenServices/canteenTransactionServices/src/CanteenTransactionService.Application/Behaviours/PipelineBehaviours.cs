using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace CanteenTransactionService.Application.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("[MediatR] Handling {RequestName}", requestName);

        var sw = Stopwatch.StartNew();
        var response = await next();
        sw.Stop();

        _logger.LogInformation("[MediatR] Handled {RequestName} in {ElapsedMs}ms", requestName, sw.ElapsedMilliseconds);
        return response;
    }
}

public class PerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<PerformanceBehaviour<TRequest, TResponse>> _logger;
    private const int SlowThresholdMs = 500;

    public PerformanceBehaviour(ILogger<PerformanceBehaviour<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var sw = Stopwatch.StartNew();
        var response = await next();
        sw.Stop();

        if (sw.ElapsedMilliseconds > SlowThresholdMs)
            _logger.LogWarning("[Performance] Slow request {RequestName}: {ElapsedMs}ms", typeof(TRequest).Name, sw.ElapsedMilliseconds);

        return response;
    }
}

public class UnhandledExceptionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<UnhandledExceptionBehaviour<TRequest, TResponse>> _logger;

    public UnhandledExceptionBehaviour(ILogger<UnhandledExceptionBehaviour<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Unhandled Exception] {RequestName}", typeof(TRequest).Name);
            throw;
        }
    }
}
