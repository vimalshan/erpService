using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace AttendanceService.Infrastructure.Polly;

public static class CircuitBreakerPolicy
{
    public static ResiliencePipeline CreatePipeline(
        int failureThreshold = 5,
        TimeSpan? samplingDuration = null,
        double failureRatio = 0.5,
        TimeSpan? breakDuration = null)
    {
        return new ResiliencePipelineBuilder()
            .AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = failureRatio,
                SamplingDuration = samplingDuration ?? TimeSpan.FromSeconds(30),
                MinimumThroughput = failureThreshold,
                BreakDuration = breakDuration ?? TimeSpan.FromSeconds(15),
                OnOpened = args =>
                {
                    Console.WriteLine($"Circuit opened for {args.BreakDuration}");
                    return ValueTask.CompletedTask;
                },
                OnClosed = _ =>
                {
                    Console.WriteLine("Circuit closed.");
                    return ValueTask.CompletedTask;
                }
            })
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(1),
                BackoffType = DelayBackoffType.Exponential
            })
            .Build();
    }
}
