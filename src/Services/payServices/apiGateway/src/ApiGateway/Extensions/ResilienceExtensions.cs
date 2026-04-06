using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;

namespace ApiGateway.Extensions;

/// <summary>
/// Configures Polly resilience policies: Circuit Breaker, Retry, Timeout, and Bulkhead Isolation.
/// </summary>
public static class ResilienceExtensions
{
    public static IServiceCollection AddResiliencePolicies(this IServiceCollection services, IConfiguration configuration)
    {
        // Circuit Breaker settings
        var cbFailureThreshold = configuration.GetValue("CircuitBreaker:FailureThreshold", 0.5);
        var cbSamplingDuration = configuration.GetValue("CircuitBreaker:SamplingDuration", 30);
        var cbMinThroughput = configuration.GetValue("CircuitBreaker:MinimumThroughput", 10);
        var cbBreakDuration = configuration.GetValue("CircuitBreaker:BreakDuration", 30);

        // Retry settings
        var retryMaxAttempts = configuration.GetValue("Retry:MaxRetryAttempts", 3);
        var retryBaseDelay = configuration.GetValue("Retry:BaseDelaySeconds", 1);

        // Timeout settings
        var timeoutSeconds = configuration.GetValue("Timeout:DefaultTimeoutSeconds", 30);

        // Bulkhead settings
        var bulkheadMaxParallel = configuration.GetValue("Bulkhead:MaxParallelization", 25);
        var bulkheadMaxQueue = configuration.GetValue("Bulkhead:MaxQueuingActions", 50);

        services.AddResiliencePipeline("gateway-pipeline", builder =>
        {
            // 1. Timeout (outermost — total time limit)
            builder.AddTimeout(new TimeoutStrategyOptions
            {
                Timeout = TimeSpan.FromSeconds(timeoutSeconds),
                Name = "GatewayTimeout",
                OnTimeout = args =>
                {
                    var logger = args.Context.Properties.GetValue(
                        new ResiliencePropertyKey<ILogger>("Logger"), null!);
                    logger?.LogWarning("Request timed out after {Timeout}s", timeoutSeconds);
                    return ValueTask.CompletedTask;
                }
            });

            // 2. Retry (exponential backoff with jitter)
            builder.AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = retryMaxAttempts,
                Delay = TimeSpan.FromSeconds(retryBaseDelay),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                ShouldHandle = new PredicateBuilder()
                    .Handle<HttpRequestException>()
                    .Handle<TimeoutRejectedException>(),
                OnRetry = args =>
                {
                    var logger = args.Context.Properties.GetValue(
                        new ResiliencePropertyKey<ILogger>("Logger"), null!);
                    logger?.LogWarning(
                        "Retry attempt {Attempt}/{MaxAttempts} after {Delay}ms",
                        args.AttemptNumber + 1, retryMaxAttempts, args.RetryDelay.TotalMilliseconds);
                    return ValueTask.CompletedTask;
                }
            });

            // 3. Circuit Breaker
            builder.AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = cbFailureThreshold,
                SamplingDuration = TimeSpan.FromSeconds(cbSamplingDuration),
                MinimumThroughput = cbMinThroughput,
                BreakDuration = TimeSpan.FromSeconds(cbBreakDuration),
                ShouldHandle = new PredicateBuilder()
                    .Handle<HttpRequestException>()
                    .Handle<TimeoutRejectedException>(),
                OnOpened = args =>
                {
                    var logger = args.Context.Properties.GetValue(
                        new ResiliencePropertyKey<ILogger>("Logger"), null!);
                    logger?.LogError("Circuit OPENED for {Duration}s — downstream service failures detected",
                        cbBreakDuration);
                    return ValueTask.CompletedTask;
                },
                OnClosed = args =>
                {
                    var logger = args.Context.Properties.GetValue(
                        new ResiliencePropertyKey<ILogger>("Logger"), null!);
                    logger?.LogInformation("Circuit CLOSED — downstream service recovered");
                    return ValueTask.CompletedTask;
                },
                OnHalfOpened = args =>
                {
                    var logger = args.Context.Properties.GetValue(
                        new ResiliencePropertyKey<ILogger>("Logger"), null!);
                    logger?.LogInformation("Circuit HALF-OPEN — testing downstream service");
                    return ValueTask.CompletedTask;
                }
            });

            // 4. Bulkhead Isolation (innermost — limits concurrent requests)
            builder.AddConcurrencyLimiter(bulkheadMaxParallel, bulkheadMaxQueue);
        });

        // Per-service HTTP client pipelines
        var services_config = new Dictionary<string, string>
        {
            ["employee-service"] = "http://localhost:5104",
            ["hr-service"] = "http://localhost:5000",
            ["faq-service"] = "http://localhost:5032",
            ["payroll-service"] = "http://localhost:5002",
            ["tax-service"] = "http://localhost:5010",
            ["paytransactional-service"] = "http://localhost:5020"
        };

        foreach (var (name, baseUrl) in services_config)
        {
            services.AddHttpClient(name, client =>
            {
                client.BaseAddress = new Uri(baseUrl);
                client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
                client.DefaultRequestHeaders.Add("X-Gateway", "ERPMicroserviceGateway");
            });
        }

        return services;
    }
}
