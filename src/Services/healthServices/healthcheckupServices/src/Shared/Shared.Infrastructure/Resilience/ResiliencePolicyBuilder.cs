namespace Shared.Infrastructure.Resilience;

using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

/// <summary>
/// Resilience policy builder for distributed system patterns
/// </summary>
public static class ResiliencePolicyBuilder
{
    /// <summary>
    /// Create a retry policy with exponential backoff
    /// </summary>
    public static IAsyncPolicy<T> CreateRetryPolicy<T>(
        int maxRetryAttempts = 3,
        double backoffMultiplier = 2.0)
    {
        return Policy
            .Handle<HttpRequestException>()
            .Or<InvalidOperationException>()
            .OrResult<T>(r => r == null)
            .WaitAndRetryAsync(
                retryCount: maxRetryAttempts,
                sleepDurationProvider: retryAttempt =>
                {
                    var delay = TimeSpan.FromSeconds(Math.Pow(backoffMultiplier, retryAttempt));
                    var jitter = new Random().Next(0, (int)delay.TotalMilliseconds);
                    return TimeSpan.FromMilliseconds(delay.TotalMilliseconds + jitter);
                },
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    Console.WriteLine($"Retry {retryCount} after {timespan.TotalSeconds}s");
                });
    }

    /// <summary>
    /// Create a circuit breaker policy
    /// </summary>
    public static IAsyncPolicy<T> CreateCircuitBreakerPolicy<T>(
        int failureThreshold = 3,
        int samplingDurationSeconds = 30,
        int breakDurationSeconds = 10)
    {
        return Policy
            .Handle<HttpRequestException>()
            .Or<InvalidOperationException>()
            .OrResult<T>(r => r == null)
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: failureThreshold,
                durationOfBreak: TimeSpan.FromSeconds(breakDurationSeconds),
                onBreak: (outcome, duration) =>
                {
                    Console.WriteLine($"Circuit breaker opened for {duration.TotalSeconds}s");
                },
                onReset: () =>
                {
                    Console.WriteLine("Circuit breaker reset");
                });
    }

    /// <summary>
    /// Create a bulkhead isolation policy
    /// </summary>
    public static IAsyncPolicy<T> CreateBulkheadPolicy<T>(
        int maxParallelization = 10,
        int maxQueuingActions = 5)
    {
        return Policy.BulkheadAsync<T>(
            maxParallelization: maxParallelization,
            maxQueuingActions: maxQueuingActions);
    }

    /// <summary>
    /// Create a combined policy with retry + circuit breaker
    /// </summary>
    public static IAsyncPolicy<T> CreateCombinedPolicy<T>(
        int maxRetryAttempts = 3,
        int failureThreshold = 5,
        int breakDurationSeconds = 30)
    {
        var retryPolicy = CreateRetryPolicy<T>(maxRetryAttempts);
        var circuitBreakerPolicy = CreateCircuitBreakerPolicy<T>(failureThreshold, breakDurationSeconds: breakDurationSeconds);

        return Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);
    }

    /// <summary>
    /// Create a timeout policy
    /// </summary>
    public static IAsyncPolicy<T> CreateTimeoutPolicy<T>(int timeoutSeconds = 30)
    {
        return Policy.TimeoutAsync<T>(TimeSpan.FromSeconds(timeoutSeconds));
    }

    /// <summary>
    /// Create a fallback policy
    /// </summary>
    public static IAsyncPolicy<T> CreateFallbackPolicy<T>(T fallbackValue)
    {
        return Policy
            .Handle<Exception>()
            .OrResult<T>(r => r == null)
            .FallbackAsync(fallbackValue);
    }
}

/// <summary>
/// Configuration for circuit breaker policies
/// </summary>
public class CircuitBreakerOptions
{
    public int FailureThreshold { get; set; } = 3;
    public int SamplingDurationSeconds { get; set; } = 30;
    public int BreakDurationSeconds { get; set; } = 10;
}

/// <summary>
/// Configuration for retry policies
/// </summary>
public class RetryOptions
{
    public int MaxRetryAttempts { get; set; } = 3;
    public double BackoffMultiplier { get; set; } = 2.0;
    public int InitialDelayMilliseconds { get; set; } = 100;
}
