using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace LocationServices.Infrastructure.Resilience;

/// <summary>
/// Polly-based resilience service providing retry + circuit-breaker policies 
/// for external service calls (DB, RabbitMQ, Blob Storage).
/// </summary>
public interface IResilienceService
{
    Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> action, CancellationToken ct = default);
    Task ExecuteAsync(Func<CancellationToken, Task> action, CancellationToken ct = default);
}

public sealed class ResilienceService : IResilienceService
{
    private readonly AsyncRetryPolicy          _retryPolicy;
    private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
    private readonly ILogger<ResilienceService> _logger;

    public ResilienceService(ILogger<ResilienceService> logger)
    {
        _logger = logger;

        // Exponential back-off retry: 3 attempts (1s, 2s, 4s)
        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt - 1)),
                onRetry: (ex, wait, attempt, _) =>
                    _logger.LogWarning("[Retry] Attempt {Attempt} after {Wait}ms — {Message}",
                        attempt, wait.TotalMilliseconds, ex.Message));

        // Circuit breaker: opens after 5 consecutive failures; stays open for 30 s
        _circuitBreakerPolicy = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak:    (ex, ts) => _logger.LogError("[Circuit] OPEN for {Seconds}s — {Message}", ts.TotalSeconds, ex.Message),
                onReset:    ()       => _logger.LogInformation("[Circuit] CLOSED — service recovered"),
                onHalfOpen: ()       => _logger.LogWarning("[Circuit] HALF-OPEN — probing..."));
    }

    public Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> action, CancellationToken ct = default) =>
        Policy.WrapAsync(_retryPolicy, _circuitBreakerPolicy)
              .ExecuteAsync(action, ct);

    public Task ExecuteAsync(Func<CancellationToken, Task> action, CancellationToken ct = default) =>
        Policy.WrapAsync(_retryPolicy, _circuitBreakerPolicy)
              .ExecuteAsync(action, ct);
}
