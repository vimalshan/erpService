namespace CompensationService.Infrastructure;

using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Service for managing Polly resilience policies.
/// </summary>
public interface IResiliencePolicies
{
    /// <summary>Gets the retry policy.</summary>
    IAsyncPolicy<HttpResponseMessage> GetHttpRetryPolicy();

    /// <summary>Gets the circuit breaker policy.</summary>
    IAsyncPolicy<HttpResponseMessage> GetHttpCircuitBreakerPolicy();

    /// <summary>Gets the combined policy with retry and circuit breaker.</summary>
    IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy();
}

/// <summary>
/// Polly resilience policies implementation.
/// </summary>
public class ResiliencePolicies : IResiliencePolicies
{
    private readonly int _retryAttempts;
    private readonly int _retryDelaySeconds;
    private readonly int _circuitBreakerThreshold;
    private readonly int _circuitBreakerTimeoutSeconds;
    private readonly ILogger<ResiliencePolicies> _logger;

    public ResiliencePolicies(IConfiguration configuration, ILogger<ResiliencePolicies> logger)
    {
        _logger = logger;
        var pollyConfig = configuration.GetSection("Polly");
        _retryAttempts = int.Parse(pollyConfig["RetryAttempts"] ?? "3");
        _retryDelaySeconds = int.Parse(pollyConfig["RetryDelaySeconds"] ?? "2");
        _circuitBreakerThreshold = int.Parse(pollyConfig["CircuitBreakerThreshold"] ?? "5");
        _circuitBreakerTimeoutSeconds = int.Parse(pollyConfig["CircuitBreakerTimeoutSeconds"] ?? "30");
    }

    public IAsyncPolicy<HttpResponseMessage> GetHttpRetryPolicy()
    {
        return Policy
            .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .Or<HttpRequestException>()
            .WaitAndRetryAsync(
                retryCount: _retryAttempts,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(_retryDelaySeconds * attempt),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    _logger.LogWarning($"Retry {retryCount} after {timespan.TotalSeconds} seconds. Reason: {outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString()}");
                });
    }

    public IAsyncPolicy<HttpResponseMessage> GetHttpCircuitBreakerPolicy()
    {
        return Policy
            .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .Or<HttpRequestException>()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: _circuitBreakerThreshold,
                durationOfBreak: TimeSpan.FromSeconds(_circuitBreakerTimeoutSeconds),
                onBreak: (outcome, timespan) =>
                {
                    _logger.LogError($"Circuit breaker opened for {timespan.TotalSeconds} seconds");
                },
                onReset: () =>
                {
                    _logger.LogInformation("Circuit breaker reset");
                });
    }

    public IAsyncPolicy<HttpResponseMessage> GetCombinedPolicy()
    {
        return Policy.WrapAsync(GetHttpRetryPolicy(), GetHttpCircuitBreakerPolicy());
    }
}

/// <summary>
/// Extension methods for resilience policies registration.
/// </summary>
public static class ResiliencePoliciesExtensions
{
    public static IServiceCollection AddResiliencePolicies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IResiliencePolicies>(sp =>
            new ResiliencePolicies(configuration, sp.GetRequiredService<ILogger<ResiliencePolicies>>()));

        return services;
    }
}
