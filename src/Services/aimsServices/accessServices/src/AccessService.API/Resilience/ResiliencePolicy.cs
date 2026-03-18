namespace AccessService.API.Resilience;

using Polly;
using Polly.CircuitBreaker;
using Microsoft.Extensions.Logging;

/// <summary>
/// Polly resilience policies for external API calls and service dependencies
/// </summary>
public static class ResiliencePolicy
{
    /// <summary>
    /// Circuit Breaker policy: Opens after 3 failures, half-opens after 30 seconds
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> CircuitBreakerPolicy(ILogger logger)
    {
        return Policy
            .Handle<HttpRequestException>()
            .Or<OperationCanceledException>()
            .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .CircuitBreakerAsync<HttpResponseMessage>(
                handledEventsAllowedBeforeBreaking: 3,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (outcome, timespan) =>
                {
                    logger.LogWarning($"Circuit breaker opened for {timespan.TotalSeconds} seconds due to failures");
                },
                onReset: () =>
                {
                    logger.LogInformation("Circuit breaker reset");
                },
                onHalfOpen: () =>
                {
                    logger.LogInformation("Circuit breaker entering half-open state");
                }
            );
    }

    /// <summary>
    /// Retry policy: Retries up to 3 times with exponential backoff (1s, 2s, 4s)
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> RetryPolicy(ILogger logger)
    {
        return Policy
            .Handle<HttpRequestException>()
            .Or<OperationCanceledException>()
            .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync<HttpResponseMessage>(
                retryCount: 3,
                sleepDurationProvider: attempt =>
                {
                    var delay = TimeSpan.FromSeconds(Math.Pow(2, attempt));
                    logger.LogWarning($"Retrying request after {delay.TotalSeconds} seconds (attempt {attempt + 1})");
                    return delay;
                },
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    logger.LogWarning($"Request failed, retrying {retryCount} times after {timespan.TotalSeconds}s");
                }
            );
    }

    /// <summary>
    /// Timeout policy: Cancels request after 10 seconds
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> TimeoutPolicy(ILogger logger)
    {
        return Policy
            .TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10),
                onTimeoutAsync: (context, timespan, _, _) =>
                {
                    logger.LogWarning($"Request timeout after {timespan.TotalSeconds} seconds");
                    return Task.CompletedTask;
                }
            );
    }

    /// <summary>
    /// Combined policy: Timeout → Retry → Circuit Breaker
    /// Uses policy wrap to execute in order: timeout first, then retry, then circuit breaker
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> CombinedResiliencePolicy(ILogger logger)
    {
        return Policy.WrapAsync(
            TimeoutPolicy(logger),
            RetryPolicy(logger),
            CircuitBreakerPolicy(logger)
        );
    }
}
