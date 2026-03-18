using Polly;
using Polly.CircuitBreaker;
using Microsoft.Extensions.Logging;

namespace MedicalVisit.Infrastructure.Resilience;

public class CircuitBreakerPolicy
{
    private readonly AsyncCircuitBreakerPolicy _policy;
    private readonly ILogger<CircuitBreakerPolicy> _logger;

    public CircuitBreakerPolicy(ILogger<CircuitBreakerPolicy> logger)
    {
        _logger = logger;

        _policy = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (exception, breakDelay) =>
                {
                    _logger.LogWarning("Circuit breaker OPEN for {BreakDelay}s due to: {Exception}",
                        breakDelay.TotalSeconds, exception.Message);
                },
                onReset: () =>
                {
                    _logger.LogInformation("Circuit breaker CLOSED - service recovered");
                },
                onHalfOpen: () =>
                {
                    _logger.LogInformation("Circuit breaker HALF-OPEN - testing service");
                });
    }

    public async Task<T> ExecuteAsync<T>(Func<Task<T>> action)
    {
        return await _policy.ExecuteAsync(action);
    }

    public async Task ExecuteAsync(Func<Task> action)
    {
        await _policy.ExecuteAsync(action);
    }
}

public static class ResilienceExtensions
{
    public static IAsyncPolicy CreateRetryWithCircuitBreaker(ILogger logger)
    {
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (exception, timeSpan, attempt, context) =>
                {
                    logger.LogWarning("Retry {Attempt} after {TimeSpan}s due to: {Exception}",
                        attempt, timeSpan.TotalSeconds, exception.Message);
                });

        var circuitBreakerPolicy = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30));

        return Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);
    }
}
