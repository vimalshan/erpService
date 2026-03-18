using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Microsoft.Extensions.Logging;

namespace DevelopmentService.Infrastructure.Resilience;

public class ResiliencePolicyProvider
{
    public static AsyncRetryPolicy GetRetryPolicy(ILogger logger) =>
        Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (exception, delay, attempt, _) =>
                    logger.LogWarning(exception,
                        "Retry {Attempt} after {Delay}s due to: {Message}",
                        attempt, delay.TotalSeconds, exception.Message));

    public static AsyncCircuitBreakerPolicy GetCircuitBreakerPolicy(ILogger logger) =>
        Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (ex, duration) =>
                    logger.LogError(ex, "Circuit broken for {Duration}s", duration.TotalSeconds),
                onReset: () =>
                    logger.LogInformation("Circuit reset."),
                onHalfOpen: () =>
                    logger.LogInformation("Circuit half-open, testing next call."));
}
