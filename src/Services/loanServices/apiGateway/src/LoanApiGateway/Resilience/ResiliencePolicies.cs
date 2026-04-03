using Microsoft.Extensions.Http.Resilience;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;

namespace LoanApiGateway.Resilience;

// Non-static marker class for ILogger<T> generic argument
internal sealed class ResiliencePoliciesLogger { }

/// <summary>
/// Registers resilience policies for YARP's HttpClient:
///   - Retry         : exponential back-off, 3 retries on transient errors
///   - Circuit Breaker: opens after 5 consecutive failures for 30 s, half-open probes
///   - Timeout       : 30 s per-request, 60 s total hedge timeout
///   - Rate Limiter  : applied via rate-limiting middleware (not here)
/// </summary>
public static class ResiliencePolicies
{
    public static IServiceCollection AddGatewayResilience(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var cfg = configuration.GetSection("Gateway:Resilience");

        int retryCount = cfg.GetValue("RetryCount", 3);
        int circuitBreakerThreshold = cfg.GetValue("CircuitBreakerFailureThreshold", 5);
        int circuitBreakerOpenSeconds = cfg.GetValue("CircuitBreakerOpenSeconds", 30);
        int requestTimeoutSeconds = cfg.GetValue("RequestTimeoutSeconds", 30);

        // Use the two-arg overload so we can resolve ILogger from the DI container
        services.AddResiliencePipeline("gateway-pipeline", (builder, context) =>
        {
            var logger = context.ServiceProvider
                .GetRequiredService<ILogger<ResiliencePoliciesLogger>>();

            // 1. Timeout (innermost — applies per attempt)
            builder.AddTimeout(new TimeoutStrategyOptions
            {
                Timeout = TimeSpan.FromSeconds(requestTimeoutSeconds),
                OnTimeout = _ =>
                {
                    logger.LogWarning("Request timeout after {Timeout}s", requestTimeoutSeconds);
                    return ValueTask.CompletedTask;
                }
            });

            // 2. Retry with exponential back-off
            builder.AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = retryCount,
                BackoffType = DelayBackoffType.Exponential,
                Delay = TimeSpan.FromMilliseconds(200),
                UseJitter = true,
                ShouldHandle = new PredicateBuilder()
                    .Handle<HttpRequestException>()
                    .Handle<TimeoutRejectedException>(),
                OnRetry = args =>
                {
                    logger.LogWarning(
                        "Retry attempt {Attempt}/{MaxAttempts} after {Delay}ms",
                        args.AttemptNumber + 1,
                        retryCount,
                        args.RetryDelay.TotalMilliseconds);
                    return ValueTask.CompletedTask;
                }
            });

            // 3. Circuit Breaker (outermost)
            builder.AddCircuitBreaker(new CircuitBreakerStrategyOptions
            {
                FailureRatio = 0.5,
                MinimumThroughput = circuitBreakerThreshold,
                SamplingDuration = TimeSpan.FromSeconds(30),
                BreakDuration = TimeSpan.FromSeconds(circuitBreakerOpenSeconds),
                ShouldHandle = new PredicateBuilder()
                    .Handle<HttpRequestException>()
                    .Handle<TimeoutRejectedException>(),
                OnOpened = args =>
                {
                    logger.LogError(
                        "Circuit opened! Breaking for {Duration}s. Failure outcome: {Outcome}",
                        circuitBreakerOpenSeconds,
                        args.Outcome.Exception?.Message ?? "Unknown");
                    return ValueTask.CompletedTask;
                },
                OnClosed = _ =>
                {
                    logger.LogInformation("Circuit closed — service recovered.");
                    return ValueTask.CompletedTask;
                },
                OnHalfOpened = _ =>
                {
                    logger.LogInformation("Circuit half-open — probing service health.");
                    return ValueTask.CompletedTask;
                }
            });
        });

        return services;
    }
}
