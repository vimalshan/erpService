using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;

namespace Recruitment.Infrastructure.Resilience;

/// <summary>
/// Polly policies for resilience
/// </summary>
public static class PollyPolicies
{
    /// <summary>
    /// Create a circuit breaker policy with exponential backoff
    /// </summary>
    public static IAsyncPolicy<T> CreateCircuitBreakerPolicy<T>(
        int handledEventsAllowedBeforeBreaking = 3,
        int durationOfBreakInSeconds = 30) where T : class
    {
        return Policy<T>
            .Handle<Exception>()
            .OrResult(r => r == null)
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking,
                TimeSpan.FromSeconds(durationOfBreakInSeconds),
                onBreak: (outcome, duration) =>
                {
                    System.Diagnostics.Debug.WriteLine($"Circuit breaker opened for {duration.TotalSeconds} seconds");
                },
                onReset: () =>
                {
                    System.Diagnostics.Debug.WriteLine("Circuit breaker reset");
                });
    }

    /// <summary>
    /// Create a retry policy with exponential backoff
    /// </summary>
    public static IAsyncPolicy<T> CreateRetryPolicy<T>(int retryCount = 3) where T : class
    {
        return Policy<T>
            .Handle<Exception>()
            .OrResult(r => r == null)
            .WaitAndRetryAsync(
                retryCount,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, duration, retryCount, context) =>
                {
                    System.Diagnostics.Debug.WriteLine($"Retry {retryCount} after {duration.TotalSeconds} seconds");
                });
    }

    /// <summary>
    /// Create a combined policy with retry and circuit breaker
    /// </summary>
    public static IAsyncPolicy<T> CreateResilientPolicy<T>(
        int retryCount = 3,
        int handledEventsAllowedBeforeBreaking = 3,
        int circuitBreakerDurationInSeconds = 30) where T : class
    {
        var retryPolicy = CreateRetryPolicy<T>(retryCount);
        var circuitBreakerPolicy = CreateCircuitBreakerPolicy<T>(
            handledEventsAllowedBeforeBreaking,
            circuitBreakerDurationInSeconds);

        return Policy.WrapAsync<T>(retryPolicy, circuitBreakerPolicy);
    }

    /// <summary>
    /// Create a timeout policy
    /// </summary>
    public static IAsyncPolicy<T> CreateTimeoutPolicy<T>(int timeoutInSeconds = 10) where T : class
    {
        return Policy.TimeoutAsync<T>(TimeSpan.FromSeconds(timeoutInSeconds));
    }
}
