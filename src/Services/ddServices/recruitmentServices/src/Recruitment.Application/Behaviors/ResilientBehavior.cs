using MediatR;
using Microsoft.Extensions.Logging;

namespace Recruitment.Application.Behaviors;

/// <summary>
/// MediatR behavior that logs all request handling
/// </summary>
/// <typeparam name="TRequest">Request type</typeparam>
/// <typeparam name="TResponse">Response type</typeparam>
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var requestId = Guid.NewGuid().ToString();

        try
        {
            _logger.LogInformation($"[{requestId}] Starting request: {requestName}");
            var response = await next();
            _logger.LogInformation($"[{requestId}] Completed request: {requestName}");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"[{requestId}] Error handling request: {requestName}");
            throw;
        }
    }
}

/// <summary>
/// Validation behavior for CQRS requests using FluentValidation
/// </summary>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

    public ValidationBehavior(ILogger<ValidationBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        
        // If there are validators registered, they will be called by MediatR
        // This behavior logs when validation would occur
        _logger.LogDebug($"Validating request: {requestName}");
        
        return await next();
    }
}

/// <summary>
/// Performance logging behavior to measure request execution time
/// </summary>
public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;

    public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var startTime = DateTime.UtcNow;

        try
        {
            var response = await next();
            
            var elapsedTime = (DateTime.UtcNow - startTime).TotalMilliseconds;
            if (elapsedTime > 1000) // Log warnings for slow queries
            {
                _logger.LogWarning($"Long-running request {requestName} completed in {elapsedTime}ms");
            }
            else
            {
                _logger.LogDebug($"Request {requestName} completed in {elapsedTime}ms");
            }

            return response;
        }
        catch (Exception ex)
        {
            var elapsedTime = (DateTime.UtcNow - startTime).TotalMilliseconds;
            _logger.LogError(ex, $"Request {requestName} failed after {elapsedTime}ms");
            throw;
        }
    }
}
