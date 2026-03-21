using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Microsoft.Extensions.Logging;

namespace BookingService.Infrastructure.Resilience;

public static class ResiliencePolicies
{
    /// <summary>
    /// HTTP retry policy: 3 retries with exponential backoff.
    /// </summary>
    public static ResiliencePipeline<HttpResponseMessage> HttpRetryPipeline(ILogger logger)
        => new ResiliencePipelineBuilder<HttpResponseMessage>()
            .AddRetry(new RetryStrategyOptions<HttpResponseMessage>
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(2),
                BackoffType = DelayBackoffType.Exponential,
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .HandleResult(r => !r.IsSuccessStatusCode),
                OnRetry = args =>
                {
                    logger.LogWarning("Retry {Attempt} after {Delay}", args.AttemptNumber, args.RetryDelay);
                    return ValueTask.CompletedTask;
                }
            })
            .Build();

    /// <summary>
    /// Circuit breaker: opens after 5 failures, stays open for 30s.
    /// </summary>
    public static ResiliencePipeline<HttpResponseMessage> HttpCircuitBreakerPipeline(ILogger logger)
        => new ResiliencePipelineBuilder<HttpResponseMessage>()
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions<HttpResponseMessage>
            {
                FailureRatio = 0.5,
                MinimumThroughput = 5,
                SamplingDuration = TimeSpan.FromSeconds(30),
                BreakDuration = TimeSpan.FromSeconds(30),
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .HandleResult(r => !r.IsSuccessStatusCode),
                OnOpened = args =>
                {
                    logger.LogWarning("Circuit breaker OPENED for {Duration}", args.BreakDuration);
                    return ValueTask.CompletedTask;
                },
                OnClosed = _ =>
                {
                    logger.LogInformation("Circuit breaker CLOSED");
                    return ValueTask.CompletedTask;
                }
            })
            .Build();

    /// <summary>
    /// Combined retry + circuit breaker pipeline for general HTTP calls.
    /// </summary>
    public static ResiliencePipeline<HttpResponseMessage> CombinedHttpPipeline(ILogger logger)
        => new ResiliencePipelineBuilder<HttpResponseMessage>()
            .AddRetry(new RetryStrategyOptions<HttpResponseMessage>
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(1),
                BackoffType = DelayBackoffType.Exponential,
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .HandleResult(r => !r.IsSuccessStatusCode)
            })
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions<HttpResponseMessage>
            {
                FailureRatio = 0.5,
                MinimumThroughput = 5,
                SamplingDuration = TimeSpan.FromSeconds(30),
                BreakDuration = TimeSpan.FromSeconds(30),
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .HandleResult(r => !r.IsSuccessStatusCode)
            })
            .Build();
}
