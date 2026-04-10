using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Timeout;
using Polly.Bulkhead;

namespace SparshApiGateway.Resilience;

/// <summary>
/// Centralized Polly resilience policies: Circuit Breaker, Retry with exponential backoff,
/// Timeout, and Bulkhead Isolation.
/// </summary>
public static class ResiliencePolicies
{
    /// <summary>
    /// Retry policy with exponential backoff + jitter.
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int maxRetries = 3, int baseDelayMs = 500)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(
                maxRetries,
                retryAttempt =>
                {
                    var delay = TimeSpan.FromMilliseconds(baseDelayMs * Math.Pow(2, retryAttempt - 1));
                    var jitter = TimeSpan.FromMilliseconds(Random.Shared.Next(0, 200));
                    return delay + jitter;
                },
                onRetry: (outcome, timespan, retryAttempt, context) =>
                {
                    var logger = context.GetLogger();
                    logger?.LogWarning(
                        "Retry {RetryAttempt} after {Delay}ms for {PolicyKey} - {Reason}",
                        retryAttempt, timespan.TotalMilliseconds,
                        context.PolicyKey, outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString());
                });
    }

    /// <summary>
    /// Circuit breaker that opens after consecutive transient failures.
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(
        int handledEventsBeforeBreaking = 5, int durationOfBreakSeconds = 15)
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsBeforeBreaking,
                TimeSpan.FromSeconds(durationOfBreakSeconds),
                onBreak: (outcome, breakDuration, context) =>
                {
                    var logger = context.GetLogger();
                    logger?.LogError(
                        "Circuit OPEN for {PolicyKey} - Duration: {BreakDuration}s - Reason: {Reason}",
                        context.PolicyKey, breakDuration.TotalSeconds,
                        outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString());
                },
                onReset: context =>
                {
                    var logger = context.GetLogger();
                    logger?.LogInformation("Circuit CLOSED for {PolicyKey}", context.PolicyKey);
                },
                onHalfOpen: () => { });
    }

    /// <summary>
    /// Timeout policy that cancels requests exceeding the threshold.
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy(int timeoutSeconds = 30)
    {
        return Policy.TimeoutAsync<HttpResponseMessage>(
            TimeSpan.FromSeconds(timeoutSeconds),
            TimeoutStrategy.Optimistic);
    }

    /// <summary>
    /// Bulkhead isolation to limit concurrent downstream calls.
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetBulkheadPolicy(
        int maxParallelization = 50, int maxQueuingActions = 25)
    {
        return Policy.BulkheadAsync<HttpResponseMessage>(
            maxParallelization,
            maxQueuingActions,
            onBulkheadRejectedAsync: context =>
            {
                // Bulkhead capacity exceeded
                return Task.CompletedTask;
            });
    }

    /// <summary>
    /// Combines all policies into a single resilience pipeline:
    /// Bulkhead → Timeout → Retry → Circuit Breaker
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy(
        int maxRetries = 3,
        int circuitBreakerThreshold = 5,
        int circuitBreakerDuration = 15,
        int timeoutSeconds = 30,
        int bulkheadParallel = 50,
        int bulkheadQueue = 25)
    {
        return Policy.WrapAsync(
            GetBulkheadPolicy(bulkheadParallel, bulkheadQueue),
            GetTimeoutPolicy(timeoutSeconds),
            GetRetryPolicy(maxRetries),
            GetCircuitBreakerPolicy(circuitBreakerThreshold, circuitBreakerDuration));
    }

    private static ILogger? GetLogger(this Context context)
    {
        if (context.TryGetValue("logger", out var logger))
            return logger as ILogger;
        return null;
    }
}
