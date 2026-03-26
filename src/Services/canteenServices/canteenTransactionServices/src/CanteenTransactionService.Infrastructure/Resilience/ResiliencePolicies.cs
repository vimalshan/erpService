using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Microsoft.Extensions.Logging;

namespace CanteenTransactionService.Infrastructure.Resilience;

public class ResiliencePolicies
{
    public ResiliencePipeline DatabasePipeline { get; }
    public ResiliencePipeline RabbitMqPipeline { get; }

    public ResiliencePolicies(ILogger<ResiliencePolicies> logger)
    {
        DatabasePipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(1),
                BackoffType = DelayBackoffType.Exponential,
                OnRetry = args =>
                {
                    logger.LogWarning("DB retry attempt {Attempt} after {Delay}ms",
                        args.AttemptNumber, args.RetryDelay.TotalMilliseconds);
                    return ValueTask.CompletedTask;
                }
            })
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = 0.5,
                MinimumThroughput = 10,
                SamplingDuration = TimeSpan.FromSeconds(30),
                BreakDuration = TimeSpan.FromSeconds(60),
                OnOpened = args =>
                {
                    logger.LogWarning("DB circuit breaker opened for {Duration}s", args.BreakDuration.TotalSeconds);
                    return ValueTask.CompletedTask;
                }
            })
            .Build();

        RabbitMqPipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 5,
                Delay = TimeSpan.FromSeconds(2),
                BackoffType = DelayBackoffType.Exponential,
                OnRetry = args =>
                {
                    logger.LogWarning("RabbitMQ retry attempt {Attempt}", args.AttemptNumber);
                    return ValueTask.CompletedTask;
                }
            })
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = 0.5,
                MinimumThroughput = 5,
                SamplingDuration = TimeSpan.FromSeconds(30),
                BreakDuration = TimeSpan.FromSeconds(90),
                OnOpened = args =>
                {
                    logger.LogWarning("RabbitMQ circuit breaker opened for {Duration}s", args.BreakDuration.TotalSeconds);
                    return ValueTask.CompletedTask;
                }
            })
            .Build();
    }
}
