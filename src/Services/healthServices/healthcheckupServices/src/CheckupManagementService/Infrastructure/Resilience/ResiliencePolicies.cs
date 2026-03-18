using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace CheckupManagementService.Infrastructure.Resilience;

/// <summary>
/// Resilience policies configuration
/// </summary>
public static class ResiliencePolicies
{
    /// <summary>
    /// Create a retry policy for API calls with exponential backoff
    /// </summary>
    public static IAsyncPolicy<T> CreateRetryPolicy<T>(ILogger? logger = null)
    {
        return Policy<T>
            .Handle<HttpRequestException>()
            .Or<OperationCanceledException>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt =>
                    TimeSpan.FromMilliseconds(Math.Pow(2, retryAttempt) * 100),
                onRetry: (outcome, timeout, retryCount, context) =>
                {
                    logger?.LogWarning(
                        "Retry attempt {retryCount} after {delayMs}ms",
                        retryCount, timeout.TotalMilliseconds);
                });
    }

    /// <summary>
    /// Create a circuit breaker policy
    /// </summary>
    public static IAsyncPolicy<T> CreateCircuitBreakerPolicy<T>(ILogger? logger = null)
    {
        return Policy<T>
            .Handle<Exception>()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (outcome, duration, context) =>
                {
                    logger?.LogError("Circuit breaker opened for {duration}s", duration.TotalSeconds);
                },
                onReset: (context) =>
                {
                    logger?.LogInformation("Circuit breaker reset");
                });
    }

    /// <summary>
    /// Create a combined retry and circuit breaker policy
    /// </summary>
    public static IAsyncPolicy<T> CreateResilientPolicy<T>(ILogger? logger = null)
    {
        var retryPolicy = CreateRetryPolicy<T>(logger);
        var circuitBreakerPolicy = CreateCircuitBreakerPolicy<T>(logger);
        
        return Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);
    }
}
