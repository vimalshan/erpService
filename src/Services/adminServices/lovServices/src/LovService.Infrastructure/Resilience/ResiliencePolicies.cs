using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace LovService.Infrastructure.Resilience;

public static class ResiliencePolicies
{
    public static AsyncRetryPolicy CreateRetryPolicy(ILogger logger, int retryCount = 3)
    {
        return Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (exception, timeSpan, attempt, _) =>
                {
                    logger.LogWarning(exception,
                        "Retry attempt {Attempt} after {Delay}s due to: {Message}",
                        attempt, timeSpan.TotalSeconds, exception.Message);
                });
    }

    public static AsyncCircuitBreakerPolicy CreateCircuitBreakerPolicy(ILogger logger,
        int exceptionsBeforeBreaking = 5, int durationOfBreakSeconds = 30)
    {
        return Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsBeforeBreaking,
                TimeSpan.FromSeconds(durationOfBreakSeconds),
                onBreak: (ex, duration) =>
                    logger.LogError(ex, "Circuit OPEN for {Duration}s: {Message}", duration.TotalSeconds, ex.Message),
                onReset: () =>
                    logger.LogInformation("Circuit CLOSED — service available"),
                onHalfOpen: () =>
                    logger.LogInformation("Circuit HALF-OPEN — testing service"));
    }

    /// <summary>
    /// Combined retry + circuit breaker policy (wrap).
    /// </summary>
    public static IAsyncPolicy CreateCombinedPolicy(ILogger logger)
    {
        var retry = CreateRetryPolicy(logger);
        var circuitBreaker = CreateCircuitBreakerPolicy(logger);
        return Policy.WrapAsync(retry, circuitBreaker);
    }
}
