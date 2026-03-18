using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace LoanApplication.Infrastructure.Resilience;

/// <summary>
/// Circuit breaker and retry policy settings loaded from configuration
/// </summary>
public class CircuitBreakerSettings
{
    public int FailureThreshold { get; set; } = 5;
    public int FailureWindow { get; set; } = 30;
    public int RecoveryTimeout { get; set; } = 60;
    public int MaxRetryAttempts { get; set; } = 3;
    public double RetryBackoffSeconds { get; set; } = 2;
}

/// <summary>
/// Named Polly resilience pipelines for the loan application service.
/// Register via <see cref="ResilienceExtensions.AddLoanApplicationResiliencePolicies"/>.
/// </summary>
public static class ResilienceExtensions
{
    public const string DatabasePipeline = "database-pipeline";
    public const string ExternalServicePipeline = "external-service-pipeline";

    public static IServiceCollection AddLoanApplicationResiliencePolicies(
        this IServiceCollection services,
        CircuitBreakerSettings settings)
    {
        // --- Database resilience pipeline ---
        // Retry 3 times with exponential back-off, then open circuit after 5 consecutive failures.
        services.AddResiliencePipeline(DatabasePipeline, (builder, ctx) =>
        {
            builder
                .AddRetry(new RetryStrategyOptions
                {
                    MaxRetryAttempts = settings.MaxRetryAttempts,
                    BackoffType = DelayBackoffType.Exponential,
                    Delay = TimeSpan.FromSeconds(settings.RetryBackoffSeconds),
                    OnRetry = args =>
                    {
                        var logger = ctx.ServiceProvider.GetRequiredService<ILogger<CircuitBreakerSettings>>();
                        logger.LogWarning(
                            "[Retry] Database attempt {Attempt} after {Delay}ms. Reason: {Reason}",
                            args.AttemptNumber + 1,
                            args.RetryDelay.TotalMilliseconds,
                            args.Outcome.Exception?.Message ?? "non-exception result");
                        return ValueTask.CompletedTask;
                    }
                })
                .AddCircuitBreaker(new CircuitBreakerStrategyOptions
                {
                    FailureRatio = settings.FailureThreshold / 10.0,         // e.g. 0.5 = 50% failure ratio
                    SamplingDuration = TimeSpan.FromSeconds(settings.FailureWindow),
                    MinimumThroughput = settings.FailureThreshold,
                    BreakDuration = TimeSpan.FromSeconds(settings.RecoveryTimeout),
                    OnOpened = args =>
                    {
                        var logger = ctx.ServiceProvider.GetRequiredService<ILogger<CircuitBreakerSettings>>();
                        logger.LogError(
                            "[CircuitBreaker:{Pipeline}] Circuit OPENED. Break duration: {Duration}s",
                            DatabasePipeline, settings.RecoveryTimeout);
                        return ValueTask.CompletedTask;
                    },
                    OnClosed = args =>
                    {
                        var logger = ctx.ServiceProvider.GetRequiredService<ILogger<CircuitBreakerSettings>>();
                        logger.LogInformation("[CircuitBreaker:{Pipeline}] Circuit CLOSED (recovered).", DatabasePipeline);
                        return ValueTask.CompletedTask;
                    },
                    OnHalfOpened = args =>
                    {
                        var logger = ctx.ServiceProvider.GetRequiredService<ILogger<CircuitBreakerSettings>>();
                        logger.LogInformation("[CircuitBreaker:{Pipeline}] Circuit HALF-OPEN — testing recovery.", DatabasePipeline);
                        return ValueTask.CompletedTask;
                    }
                });
        });

        // --- External service resilience pipeline (e.g. eligibility API, Azure Blob) ---
        services.AddResiliencePipeline(ExternalServicePipeline, (builder, ctx) =>
        {
            builder
                .AddRetry(new RetryStrategyOptions
                {
                    MaxRetryAttempts = 2,
                    BackoffType = DelayBackoffType.Constant,
                    Delay = TimeSpan.FromSeconds(1),
                    OnRetry = args =>
                    {
                        var logger = ctx.ServiceProvider.GetRequiredService<ILogger<CircuitBreakerSettings>>();
                        logger.LogWarning(
                            "[Retry] External service attempt {Attempt}. Reason: {Reason}",
                            args.AttemptNumber + 1,
                            args.Outcome.Exception?.Message ?? "non-exception result");
                        return ValueTask.CompletedTask;
                    }
                })
                .AddCircuitBreaker(new CircuitBreakerStrategyOptions
                {
                    FailureRatio = 0.5,
                    SamplingDuration = TimeSpan.FromSeconds(settings.FailureWindow),
                    MinimumThroughput = 3,
                    BreakDuration = TimeSpan.FromSeconds(settings.RecoveryTimeout),
                    OnOpened = args =>
                    {
                        var logger = ctx.ServiceProvider.GetRequiredService<ILogger<CircuitBreakerSettings>>();
                        logger.LogError("[CircuitBreaker:{Pipeline}] Circuit OPENED.", ExternalServicePipeline);
                        return ValueTask.CompletedTask;
                    },
                    OnClosed = args =>
                    {
                        var logger = ctx.ServiceProvider.GetRequiredService<ILogger<CircuitBreakerSettings>>();
                        logger.LogInformation("[CircuitBreaker:{Pipeline}] Circuit CLOSED.", ExternalServicePipeline);
                        return ValueTask.CompletedTask;
                    }
                });
        });

        return services;
    }
}
