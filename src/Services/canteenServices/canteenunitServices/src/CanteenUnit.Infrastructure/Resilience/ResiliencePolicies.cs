using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Wrap;

namespace CanteenUnit.Infrastructure.Resilience;

public static class ResiliencePolicies
{
    public static AsyncRetryPolicy GetRetryPolicy(ILogger logger) =>
        Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (ex, delay, attempt, _) =>
                    logger.LogWarning(ex, "Retry {Attempt} after {Delay}s", attempt, delay.TotalSeconds));

    public static AsyncCircuitBreakerPolicy GetCircuitBreakerPolicy(ILogger logger) =>
        Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (ex, ts) => logger.LogError(ex, "Circuit breaker OPEN for {Seconds}s", ts.TotalSeconds),
                onReset: () => logger.LogInformation("Circuit breaker RESET"),
                onHalfOpen: () => logger.LogInformation("Circuit breaker HALF-OPEN"));

    public static AsyncPolicyWrap GetCombinedPolicy(ILogger logger) =>
        Policy.WrapAsync(GetRetryPolicy(logger), GetCircuitBreakerPolicy(logger));
}
