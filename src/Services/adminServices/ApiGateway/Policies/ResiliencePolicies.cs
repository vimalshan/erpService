using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;
using Polly.Bulkhead;

namespace ApiGateway.Policies;

/// <summary>
/// Polly resilience policies for HTTP client calls with circuit breaker, retry, timeout, and bulkhead patterns
/// </summary>
public static class ResiliencePolicies
{
    /// <summary>
    /// Retry policy: Retry up to 3 times with exponential backoff
    /// Waits: 1s, 2s, 4s before each retry
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return Policy
            .Handle<HttpRequestException>()
            .Or<TimeoutRejectedException>()
            .OrResult<HttpResponseMessage>(r =>
                r.StatusCode == System.Net.HttpStatusCode.RequestTimeout ||
                r.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable ||
                r.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    Console.WriteLine($"Retry {retryCount} after {timespan.TotalSeconds}s due to: {outcome.Exception?.Message}");
                });
    }

    /// <summary>
    /// Circuit breaker policy: Opens after 5 consecutive failures for 30 seconds
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r =>
                !r.IsSuccessStatusCode &&
                (r.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable ||
                 r.StatusCode == System.Net.HttpStatusCode.GatewayTimeout ||
                 r.StatusCode == System.Net.HttpStatusCode.RequestTimeout))
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (outcome, timespan) =>
                {
                    Console.WriteLine($"Circuit breaker opened for {timespan.TotalSeconds}s");
                },
                onReset: () =>
                {
                    Console.WriteLine("Circuit breaker closed");
                });
    }

    /// <summary>
    /// Timeout policy: 10 seconds timeout for HTTP requests
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy()
    {
        return Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10));
    }

    /// <summary>
    /// Bulkhead policy: Maximum 10 parallel requests per service
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetBulkheadPolicy()
    {
        return Policy.BulkheadAsync<HttpResponseMessage>(
            maxParallelization: 10,
            maxQueuingActions: 20,
            onBulkheadRejectedAsync: (context) =>
            {
                Console.WriteLine("Bulkhead policy rejected request - max parallel requests reached");
                return Task.CompletedTask;
            });
    }

    /// <summary>
    /// Combined policy: Applies timeout, then circuit breaker, then retry
    /// Order: Retry -> CircuitBreaker -> Timeout -> Bulkhead
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy()
    {
        return Policy.WrapAsync(
            GetRetryPolicy(),
            GetCircuitBreakerPolicy(),
            GetTimeoutPolicy(),
            GetBulkheadPolicy());
    }

    /// <summary>
    /// Alternative combined policy: Applies timeout and retry only (for less critical services)
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetSimpleCombinedPolicy()
    {
        return Policy.WrapAsync(
            GetRetryPolicy(),
            GetTimeoutPolicy());
    }

    /// <summary>
    /// High-throughput policy: For services that can handle high load
    /// Increased bulkhead and retry limits
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetHighThroughputPolicy()
    {
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .Or<TimeoutRejectedException>()
            .OrResult<HttpResponseMessage>(r =>
                r.StatusCode == System.Net.HttpStatusCode.RequestTimeout ||
                r.StatusCode == System.Net.HttpStatusCode.ServiceUnavailable)
            .WaitAndRetryAsync(
                retryCount: 5,
                sleepDurationProvider: retryAttempt =>
                    TimeSpan.FromMilliseconds(100 * Math.Pow(2, retryAttempt)));

        var circuitBreakerPolicy = Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 10,
                durationOfBreak: TimeSpan.FromSeconds(30));

        var bulkheadPolicy = Policy.BulkheadAsync<HttpResponseMessage>(
            maxParallelization: 20,
            maxQueuingActions: 50);

        return Policy.WrapAsync(
            retryPolicy,
            circuitBreakerPolicy,
            GetTimeoutPolicy(),
            bulkheadPolicy);
    }
}
