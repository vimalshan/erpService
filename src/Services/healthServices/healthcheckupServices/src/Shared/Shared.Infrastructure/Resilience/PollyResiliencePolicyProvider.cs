using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;

namespace Shared.Infrastructure.Resilience;

/// <summary>
/// Resilience policy factory for implementing fault-tolerance patterns
/// </summary>
public interface IResiliencePolicyProvider
{
    IAsyncPolicy<T> GetRetryPolicy<T>();
    IAsyncPolicy<T> GetCircuitBreakerPolicy<T>();
    IAsyncPolicy<T> GetTimeoutPolicy<T>();
    IAsyncPolicy<T> GetBulkheadPolicy<T>();
    IAsyncPolicy<T> GetCombinedPolicy<T>();
}

public class PollyResiliencePolicyProvider : IResiliencePolicyProvider
{
    private readonly int _retryCount;
    private readonly TimeSpan _retryDelay;
    private readonly int _circuitBreakerThreshold;
    private readonly TimeSpan _circuitBreakerTimeout;
    private readonly TimeSpan _timeoutDuration;
    private readonly int _bulkheadParallelization;

    public PollyResiliencePolicyProvider(
        int retryCount = 3,
        int retryDelayMs = 100,
        int circuitBreakerThreshold = 5,
        int circuitBreakerTimeoutSec = 30,
        int timeoutSec = 10,
        int bulkheadParallelization = 10)
    {
        _retryCount = retryCount;
        _retryDelay = TimeSpan.FromMilliseconds(retryDelayMs);
        _circuitBreakerThreshold = circuitBreakerThreshold;
        _circuitBreakerTimeout = TimeSpan.FromSeconds(circuitBreakerTimeoutSec);
        _timeoutDuration = TimeSpan.FromSeconds(timeoutSec);
        _bulkheadParallelization = bulkheadParallelization;
    }

    public IAsyncPolicy<T> GetRetryPolicy<T>()
    {
        return Policy<T>
            .Handle<Exception>()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(
                retryCount: _retryCount,
                sleepDurationProvider: _ => _retryDelay,
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    // Log retry attempt
                    Console.WriteLine($"Retry {retryCount} after {timespan.TotalMilliseconds}ms");
                });
    }

    public IAsyncPolicy<T> GetCircuitBreakerPolicy<T>()
    {
        return Policy
            .Handle<Exception>()
            .OrResult<T>(r => r == null)
            .CircuitBreakerAsync<T>(
                handledEventsAllowedBeforeBreaking: _circuitBreakerThreshold,
                durationOfBreak: _circuitBreakerTimeout,
                onBreak: (outcome, duration) =>
                {
                    Console.WriteLine($"Circuit breaker opened for {duration.TotalSeconds}s");
                },
                onReset: () =>
                {
                    Console.WriteLine("Circuit breaker reset");
                });
    }

    public IAsyncPolicy<T> GetTimeoutPolicy<T>()
    {
        return Policy.TimeoutAsync<T>(_timeoutDuration, TimeoutStrategy.Pessimistic);
    }

    public IAsyncPolicy<T> GetBulkheadPolicy<T>()
    {
        return Policy.BulkheadAsync<T>(_bulkheadParallelization);
    }

    public IAsyncPolicy<T> GetCombinedPolicy<T>()
    {
        var retry = GetRetryPolicy<T>();
        var circuitBreaker = GetCircuitBreakerPolicy<T>();
        var timeout = GetTimeoutPolicy<T>();
        var bulkhead = GetBulkheadPolicy<T>();

        return Policy.WrapAsync(retry, circuitBreaker, timeout, bulkhead);
    }
}
