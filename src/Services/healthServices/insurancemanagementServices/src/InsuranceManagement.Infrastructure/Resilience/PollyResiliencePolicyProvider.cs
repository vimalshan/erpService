using Polly;
using Polly.Timeout;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace InsuranceManagement.Infrastructure.Resilience;

/// <summary>
/// Configuration for circuit breaker policies
/// </summary>
public class CircuitBreakerConfiguration
{
    public int HandledEventsAllowedBeforeBreaking { get; set; } = 3;
    public int DurationOfBreakInSeconds { get; set; } = 30;
    public int RetryAttempts { get; set; } = 3;
    public int RetryDelayMilliseconds { get; set; } = 1000;
    public int TimeoutSeconds { get; set; } = 30;
}

/// <summary>
/// Provides resilience policies using Polly
/// </summary>
public interface IResiliencePolicyProvider
{
    /// <summary>
    /// Get the database retry policy
    /// </summary>
    IAsyncPolicy<T> GetDatabaseRetryPolicy<T>();

    /// <summary>
    /// Get the circuit breaker policy
    /// </summary>
    IAsyncPolicy<T> GetCircuitBreakerPolicy<T>();

    /// <summary>
    /// Get the combined retry + circuit breaker policy
    /// </summary>
    IAsyncPolicy<T> GetCombinedPolicy<T>();

    /// <summary>
    /// Get the timeout policy
    /// </summary>
    IAsyncPolicy<T> GetTimeoutPolicy<T>();
}

/// <summary>
/// Default implementation of resilience policy provider
/// </summary>
public class PollyResiliencePolicyProvider : IResiliencePolicyProvider
{
    private readonly CircuitBreakerConfiguration _configuration;
    private readonly ILogger<PollyResiliencePolicyProvider> _logger;

    public PollyResiliencePolicyProvider(
        CircuitBreakerConfiguration configuration,
        ILogger<PollyResiliencePolicyProvider> logger)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public IAsyncPolicy<T> GetDatabaseRetryPolicy<T>()
    {
        return Policy
            .Handle<Exception>()
            .OrResult<T>(r => r == null)
            .WaitAndRetryAsync(
                retryCount: _configuration.RetryAttempts,
                sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(
                    _configuration.RetryDelayMilliseconds * (int)Math.Pow(2, attempt - 1)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    _logger.LogWarning(
                        $"Retry attempt {retryCount} after {timespan.TotalMilliseconds}ms");
                });
    }

    public IAsyncPolicy<T> GetCircuitBreakerPolicy<T>()
    {
        return Policy
            .Handle<Exception>()
            .OrResult<T>(r => r == null)
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: _configuration.HandledEventsAllowedBeforeBreaking,
                durationOfBreak: TimeSpan.FromSeconds(_configuration.DurationOfBreakInSeconds),
                onBreak: (outcome, timespan) =>
                {
                    _logger.LogError(
                        $"Circuit breaker opened. Will try again after {timespan.TotalSeconds} seconds");
                },
                onReset: () =>
                {
                    _logger.LogInformation("Circuit breaker reset");
                },
                onHalfOpen: () =>
                {
                    _logger.LogInformation("Circuit breaker is in half-open state");
                });
    }

    public IAsyncPolicy<T> GetCombinedPolicy<T>()
    {
        var retryPolicy = GetDatabaseRetryPolicy<T>();
        var circuitBreakerPolicy = GetCircuitBreakerPolicy<T>();

        return Policy.WrapAsync(retryPolicy, circuitBreakerPolicy);
    }

    public IAsyncPolicy<T> GetTimeoutPolicy<T>()
    {
        return Policy.TimeoutAsync<T>(
            timeout: TimeSpan.FromSeconds(_configuration.TimeoutSeconds),
            timeoutStrategy: TimeoutStrategy.Optimistic);
    }
}

/// <summary>
/// Extension methods for resilience policies
/// </summary>
public static class ResiliencePolicyExtensions
{
    /// <summary>
    /// Add resilience policies to the service collection
    /// </summary>
    public static IServiceCollection AddResiliencePolicies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var config = new CircuitBreakerConfiguration();
        var section = configuration.GetSection("CircuitBreaker");
        try
        {
            if (section != null && section.GetChildren().Any())
            {
                section.Bind(config);
            }
        }
        catch
        {
            // Use defaults if binding fails
        }

        services.AddSingleton(config);
        services.AddSingleton<IResiliencePolicyProvider, PollyResiliencePolicyProvider>();

        return services;
    }
}
