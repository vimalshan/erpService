using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;
using Polly.Bulkhead;
using System.Collections.Concurrent;

namespace ApiGateway.Resilience;

public static class ResiliencePolicies
{
    private static readonly ConcurrentDictionary<string, ResiliencePipeline<HttpResponseMessage>> _pipelines = new();

    public static IServiceCollection AddResiliencePolicies(this IServiceCollection services, IConfiguration config)
    {
        var circuitBreakerConfig = config.GetSection("CircuitBreaker");
        var retryConfig = config.GetSection("RetryPolicy");
        var bulkheadConfig = config.GetSection("BulkheadPolicy");

        var failureThreshold = circuitBreakerConfig.GetValue("FailureThreshold", 0.5);
        var samplingDuration = TimeSpan.FromSeconds(circuitBreakerConfig.GetValue("SamplingDurationSeconds", 30));
        var minimumThroughput = circuitBreakerConfig.GetValue("MinimumThroughput", 10);
        var breakDuration = TimeSpan.FromSeconds(circuitBreakerConfig.GetValue("BreakDurationSeconds", 15));

        var maxRetries = retryConfig.GetValue("MaxRetries", 3);
        var baseDelay = TimeSpan.FromMilliseconds(retryConfig.GetValue("BaseDelayMs", 200));
        var timeout = TimeSpan.FromSeconds(retryConfig.GetValue("TimeoutSeconds", 30));

        var maxParallelization = bulkheadConfig.GetValue("MaxParallelization", 50);
        var maxQueuingActions = bulkheadConfig.GetValue("MaxQueuingActions", 25);

        services.AddSingleton(sp =>
        {
            return new ResiliencePolicyFactory(
                failureThreshold, samplingDuration, minimumThroughput, breakDuration,
                maxRetries, baseDelay, timeout,
                maxParallelization, maxQueuingActions,
                sp.GetRequiredService<ILoggerFactory>());
        });

        return services;
    }
}

public class ResiliencePolicyFactory
{
    private readonly double _failureThreshold;
    private readonly TimeSpan _samplingDuration;
    private readonly int _minimumThroughput;
    private readonly TimeSpan _breakDuration;
    private readonly int _maxRetries;
    private readonly TimeSpan _baseDelay;
    private readonly TimeSpan _timeout;
    private readonly int _maxParallelization;
    private readonly int _maxQueuingActions;
    private readonly ILoggerFactory _loggerFactory;
    private readonly ConcurrentDictionary<string, ResiliencePipeline<HttpResponseMessage>> _pipelines = new();

    public ResiliencePolicyFactory(
        double failureThreshold, TimeSpan samplingDuration, int minimumThroughput, TimeSpan breakDuration,
        int maxRetries, TimeSpan baseDelay, TimeSpan timeout,
        int maxParallelization, int maxQueuingActions,
        ILoggerFactory loggerFactory)
    {
        _failureThreshold = failureThreshold;
        _samplingDuration = samplingDuration;
        _minimumThroughput = minimumThroughput;
        _breakDuration = breakDuration;
        _maxRetries = maxRetries;
        _baseDelay = baseDelay;
        _timeout = timeout;
        _maxParallelization = maxParallelization;
        _maxQueuingActions = maxQueuingActions;
        _loggerFactory = loggerFactory;
    }

    public ResiliencePipeline<HttpResponseMessage> GetOrCreatePipeline(string serviceName)
    {
        return _pipelines.GetOrAdd(serviceName, name =>
        {
            var logger = _loggerFactory.CreateLogger($"Resilience.{name}");

            return new ResiliencePipelineBuilder<HttpResponseMessage>()
                // Timeout
                .AddTimeout(new TimeoutStrategyOptions
                {
                    Timeout = _timeout,
                    OnTimeout = args =>
                    {
                        logger.LogWarning("Timeout for {Service} after {Timeout}s", name, _timeout.TotalSeconds);
                        return default;
                    }
                })
                // Retry with exponential backoff
                .AddRetry(new RetryStrategyOptions<HttpResponseMessage>
                {
                    MaxRetryAttempts = _maxRetries,
                    Delay = _baseDelay,
                    BackoffType = DelayBackoffType.Exponential,
                    ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                        .Handle<HttpRequestException>()
                        .Handle<TimeoutRejectedException>()
                        .HandleResult(r => (int)r.StatusCode >= 500),
                    OnRetry = args =>
                    {
                        logger.LogWarning("Retry {Attempt}/{Max} for {Service} after {Delay}ms",
                            args.AttemptNumber + 1, _maxRetries, name, args.RetryDelay.TotalMilliseconds);
                        return default;
                    }
                })
                // Circuit Breaker
                .AddCircuitBreaker(new CircuitBreakerStrategyOptions<HttpResponseMessage>
                {
                    FailureRatio = _failureThreshold,
                    SamplingDuration = _samplingDuration,
                    MinimumThroughput = _minimumThroughput,
                    BreakDuration = _breakDuration,
                    ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                        .Handle<HttpRequestException>()
                        .Handle<TimeoutRejectedException>()
                        .HandleResult(r => (int)r.StatusCode >= 500),
                    OnOpened = args =>
                    {
                        logger.LogError("Circuit OPEN for {Service} — breaking for {Duration}s", name, _breakDuration.TotalSeconds);
                        return default;
                    },
                    OnClosed = args =>
                    {
                        logger.LogInformation("Circuit CLOSED for {Service} — recovered", name);
                        return default;
                    },
                    OnHalfOpened = args =>
                    {
                        logger.LogWarning("Circuit HALF-OPEN for {Service} — testing", name);
                        return default;
                    }
                })
                .Build();
        });
    }

    // Expose configuration for health reporting
    public (int MaxParallelization, int MaxQueuingActions) BulkheadConfig => (_maxParallelization, _maxQueuingActions);
}
