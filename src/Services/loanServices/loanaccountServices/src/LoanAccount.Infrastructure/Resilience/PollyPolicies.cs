using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;

namespace LoanAccount.Infrastructure.Resilience;

/// <summary>
/// Polly resilience policy configurations
/// </summary>
public static class PollyPolicies
{
    /// <summary>
    /// Create a retry policy
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount = 3)
    {
        return Policy
            .Handle<HttpRequestException>()
            .Or<TimeoutRejectedException>()
            .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(
                retryCount,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    Console.WriteLine($"Retry {retryCount} after {timespan.TotalSeconds}s");
                });
    }

    /// <summary>
    /// Create a circuit breaker policy
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(
        int handledEventsAllowedBeforeBreaking = 5,
        int durationOfBreakInSeconds = 30)
    {
        return Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking,
                TimeSpan.FromSeconds(durationOfBreakInSeconds),
                onBreak: (outcome, timespan) =>
                {
                    Console.WriteLine($"Circuit breaker opened for {timespan.TotalSeconds}s");
                },
                onReset: () =>
                {
                    Console.WriteLine("Circuit breaker reset");
                });
    }

    /// <summary>
    /// Create a timeout policy
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy(int timeoutInSeconds = 10)
    {
        return Policy
            .TimeoutAsync<HttpResponseMessage>(
                TimeSpan.FromSeconds(timeoutInSeconds),
                TimeoutStrategy.Optimistic);
    }

    /// <summary>
    /// Combine retry, circuit breaker, and timeout policies
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy(
        int retryCount = 3,
        int circuitBreakerThreshold = 5,
        int circuitBreakerDurationSeconds = 30,
        int timeoutSeconds = 10)
    {
        return Policy.WrapAsync(
            GetRetryPolicy(retryCount),
            GetCircuitBreakerPolicy(circuitBreakerThreshold, circuitBreakerDurationSeconds),
            GetTimeoutPolicy(timeoutSeconds));
    }
}
