using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;

namespace ItemMasterService.Infrastructure.Resilience;

public class CircuitBreakerPolicy
{
    private readonly ResiliencePipeline _pipeline;
    private readonly ILogger<CircuitBreakerPolicy> _logger;

    public CircuitBreakerPolicy(ILogger<CircuitBreakerPolicy> logger)
    {
        _logger = logger;

        _pipeline = new ResiliencePipelineBuilder()
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = 0.5,
                MinimumThroughput = 10,
                SamplingDuration = TimeSpan.FromSeconds(30),
                BreakDuration = TimeSpan.FromSeconds(60),
                OnOpened = args =>
                {
                    _logger.LogWarning("[CircuitBreaker] Circuit opened. Duration: {Duration}", args.BreakDuration);
                    return ValueTask.CompletedTask;
                },
                OnClosed = _ =>
                {
                    _logger.LogInformation("[CircuitBreaker] Circuit closed.");
                    return ValueTask.CompletedTask;
                },
                OnHalfOpened = _ =>
                {
                    _logger.LogInformation("[CircuitBreaker] Circuit half-opened.");
                    return ValueTask.CompletedTask;
                }
            })
            .AddRetry(new Polly.Retry.RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(500),
                BackoffType = DelayBackoffType.Exponential,
                OnRetry = args =>
                {
                    _logger.LogWarning("[Retry] Attempt {AttemptNumber}: {Outcome}", args.AttemptNumber + 1, args.Outcome.Exception?.Message);
                    return ValueTask.CompletedTask;
                }
            })
            .Build();
    }

    public async Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> operation, CancellationToken ct = default) =>
        await _pipeline.ExecuteAsync(async token => await operation(token), ct);

    public async Task ExecuteAsync(Func<CancellationToken, Task> operation, CancellationToken ct = default) =>
        await _pipeline.ExecuteAsync(async token => await operation(token), ct);
}
