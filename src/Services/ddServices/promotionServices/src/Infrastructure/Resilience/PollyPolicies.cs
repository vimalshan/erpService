using Polly;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Retry;
using Polly.Timeout;
using Microsoft.Extensions.Logging;

namespace PromotionService.Infrastructure.Resilience;

/// <summary>
/// Polly resilience policies for HTTP clients and external service calls.
/// </summary>
public static class PollyPolicies
{
    /// <summary>Retry policy: 3 retries with exponential back-off (2s, 4s, 8s).</summary>
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(ILogger logger) =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (outcome, delay, attempt, _) =>
                    logger.LogWarning(
                        "HTTP retry {Attempt} after {Delay}s. Reason: {Reason}",
                        attempt, delay.TotalSeconds, outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString()));

    /// <summary>
    /// Circuit-breaker policy: opens after 5 consecutive failures,
    /// stays open for 30 seconds before allowing a single probe request.
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy(ILogger logger) =>
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (outcome, duration) =>
                    logger.LogError(
                        "Circuit breaker OPENED for {Duration}s. Reason: {Reason}",
                        duration.TotalSeconds, outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString()),
                onReset: () => logger.LogInformation("Circuit breaker RESET."),
                onHalfOpen: () => logger.LogWarning("Circuit breaker HALF-OPEN."));

    /// <summary>Timeout policy: 30 seconds per request.</summary>
    public static IAsyncPolicy<HttpResponseMessage> GetTimeoutPolicy() =>
        Policy.TimeoutAsync<HttpResponseMessage>(30);

    /// <summary>
    /// Combined policy: timeout → retry → circuit-breaker (wrap outermost first).
    /// </summary>
    public static IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy(ILogger logger) =>
        Policy.WrapAsync(
            GetRetryPolicy(logger),
            GetCircuitBreakerPolicy(logger),
            GetTimeoutPolicy());
}

/// <summary>
/// Polly policies for database operations (non-HTTP), e.g., Dapper queries.
/// </summary>
public static class DbResiliencePolicies
{
    /// <summary>Retry transient SQL errors 3 times with 2-second fixed delays.</summary>
    public static IAsyncPolicy GetDbRetryPolicy(ILogger logger) =>
        Policy
            .Handle<Microsoft.Data.SqlClient.SqlException>(ex => IsTransient(ex))
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: _ => TimeSpan.FromSeconds(2),
                onRetry: (ex, delay, attempt, _) =>
                    logger.LogWarning(ex, "DB retry {Attempt} after {Delay}s", attempt, delay.TotalSeconds));

    private static bool IsTransient(Microsoft.Data.SqlClient.SqlException ex) =>
        ex.Number is -2 or 20 or 64 or 233 or 10053 or 10054 or 10060 or 40197 or 40501 or 40613;
}
