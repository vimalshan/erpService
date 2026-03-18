using Polly;
using Polly.CircuitBreaker;

namespace Todos.Infrastructure.MessageBrokers;

/// <summary>
/// Extension methods for configuring Polly policies
/// </summary>
public static class PollyPolicies
{
    /// <summary>
    /// Creates a resilience policy with retry and circuit breaker
    /// </summary>
    public static IAsyncPolicy<T> GetDefaultPolicy<T>(int retryCount = 3, int circuitBreakerThreshold = 5) where T : class
    {
        return Policy<T>
            .Handle<HttpRequestException>()
            .Or<TimeoutException>()
            .OrResult(r => r == null)
            .FallbackAsync((_) => Task.FromResult((T?)null))
            .WrapAsync(Policy<T>
                .Handle<HttpRequestException>()
                .Or<TimeoutException>()
                .OrResult(r => r == null)
                .CircuitBreakerAsync<T>(
                    handledEventsAllowedBeforeBreaking: circuitBreakerThreshold,
                    durationOfBreak: TimeSpan.FromSeconds(30))
            )
            .WrapAsync(Policy<T>
                .Handle<HttpRequestException>()
                .Or<TimeoutException>()
                .OrResult(r => r == null)
                .WaitAndRetryAsync<T>(
                    retryCount: retryCount,
                    sleepDurationProvider: retryAttempt =>
                        TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                )
            );
    }

    /// <summary>
    /// Creates a simple retry policy
    /// </summary>
    public static IAsyncPolicy<T> GetRetryPolicy<T>(int retryCount = 3) where T : class
    {
        return Policy<T>
            .Handle<HttpRequestException>()
            .Or<TimeoutException>()
            .WaitAndRetryAsync<T>(
                retryCount: retryCount,
                sleepDurationProvider: retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
            );
    }

    /// <summary>
    /// Creates a circuit breaker policy
    /// </summary>
    public static IAsyncPolicy<T> GetCircuitBreakerPolicy<T>(int failureThreshold = 5, int durationSeconds = 30) where T : class
    {
        return Policy<T>
            .Handle<HttpRequestException>()
            .Or<TimeoutException>()
            .OrResult(r => r == null)
            .CircuitBreakerAsync<T>(
                handledEventsAllowedBeforeBreaking: failureThreshold,
                durationOfBreak: TimeSpan.FromSeconds(durationSeconds)
            );
    }
}
