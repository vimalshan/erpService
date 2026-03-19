using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;

namespace UserService.Infrastructure.Policies;

/// <summary>
/// Polly Circuit Breaker Policies
/// </summary>
public static class CircuitBreakerPolicies
{
    /// <summary>
    /// Circuit breaker policy for HTTP calls
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetHttpCircuitBreakerPolicy()
    {
        return Policy
            .Handle<HttpRequestException>()
            .Or<TimeoutException>()
            .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .CircuitBreakerAsync<HttpResponseMessage>(
                handledEventsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (outcome, timespan) =>
                {
                    var reason = outcome.Exception?.Message
                        ?? outcome.Result?.StatusCode.ToString()
                        ?? "Unknown";
                    System.Diagnostics.Debug.WriteLine(
                        $"Circuit breaker opened. Reason: {reason}. Will retry after {timespan.TotalSeconds} seconds");
                },
                onReset: () =>
                {
                    System.Diagnostics.Debug.WriteLine("Circuit breaker reset");
                });
    }

    /// <summary>
    /// Retry policy with exponential backoff
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return Policy
            .Handle<HttpRequestException>()
            .Or<TimeoutException>()
            .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode && r.StatusCode != System.Net.HttpStatusCode.BadRequest)
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    System.Diagnostics.Debug.WriteLine(
                        $"Retry {retryCount} after {timespan.TotalSeconds} seconds");
                });
    }

    /// <summary>
    /// Combined policy with circuit breaker and retry
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy()
    {
        var retryPolicy = GetRetryPolicy();
        var circuitBreakerPolicy = GetHttpCircuitBreakerPolicy();

        return Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);
    }

    /// <summary>
    /// Timeout policy
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy(int timeoutSeconds = 10)
    {
        return Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(timeoutSeconds));
    }

    /// <summary>
    /// Bulkhead isolation policy
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetBulkheadPolicy(int maxParallelizations = 12)
    {
        return Policy.BulkheadAsync<HttpResponseMessage>(maxParallelizations, 100);
    }
}
