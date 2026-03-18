using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace MasterService.Infrastructure.Resilience;

public static class PollyPolicies
{
    /// <summary>Retry policy: 3 retries with exponential back-off.</summary>
    public static AsyncRetryPolicy GetDatabaseRetryPolicy(ILogger logger) =>
        Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (ex, delay, attempt, ctx) =>
                    logger.LogWarning(ex, "DB retry {Attempt} in {Delay}s", attempt, delay.TotalSeconds));

    /// <summary>Circuit Breaker: opens after 5 failures in 30s, stays open for 60s.</summary>
    public static AsyncCircuitBreakerPolicy GetCircuitBreakerPolicy(ILogger logger) =>
        Policy
            .Handle<Exception>()
            .AdvancedCircuitBreakerAsync(
                failureThreshold: 0.5,
                samplingDuration: TimeSpan.FromSeconds(30),
                minimumThroughput: 5,
                durationOfBreak: TimeSpan.FromSeconds(60),
                onBreak: (ex, ts) =>
                    logger.LogError(ex, "Circuit breaker OPEN for {Duration}s", ts.TotalSeconds),
                onReset: () => logger.LogInformation("Circuit breaker CLOSED (reset)."),
                onHalfOpen: () => logger.LogInformation("Circuit breaker HALF-OPEN (testing)."));

    /// <summary>Wrap retry + circuit breaker together.</summary>
    public static AsyncPolicy GetResilientPolicy(ILogger logger) =>
        Policy.WrapAsync(GetCircuitBreakerPolicy(logger), GetDatabaseRetryPolicy(logger));
}
