using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;
using Microsoft.Extensions.Logging;

namespace EmailNotification.Infrastructure.Resilience;

/// <summary>
/// Policy keys for resilience patterns
/// </summary>
public static class PolicyKeys
{
    /// <summary>Policy for database operations</summary>
    public const string DatabasePolicy = "DatabasePolicy";

    /// <summary>Policy for external HTTP calls</summary>
    public const string HttpPolicy = "HttpPolicy";

    /// <summary>Policy for RabbitMQ operations</summary>
    public const string RabbitMqPolicy = "RabbitMqPolicy";

    /// <summary>Policy for SMTP operations</summary>
    public const string SmtpPolicy = "SmtpPolicy";
}

/// <summary>
/// Registry for managing resilience policies
/// </summary>
public interface IPolicyRegistry
{
    /// <summary>Gets a policy by its key</summary>
    IAsyncPolicy<TResult>? GetPolicy<TResult>(string policyKey);

    /// <summary>Gets a policy without result type</summary>
    IAsyncPolicy? GetPolicy(string policyKey);
}

/// <summary>
/// Implementation of policy registry using Polly
/// </summary>
public class PollyPolicyRegistry : IPolicyRegistry
{
    private readonly ILogger<PollyPolicyRegistry> _logger;
    private readonly Dictionary<string, object> _policies;

    public PollyPolicyRegistry(ILogger<PollyPolicyRegistry> logger)
    {
        _logger = logger;
        _policies = new Dictionary<string, object>();
        InitializePolicies();
    }

    /// <summary>Gets a policy by key with result type</summary>
    public IAsyncPolicy<TResult>? GetPolicy<TResult>(string policyKey)
    {
        if (_policies.TryGetValue(policyKey, out var policy))
        {
            return policy as IAsyncPolicy<TResult>;
        }

        _logger.LogWarning("Policy not found for key: {PolicyKey}", policyKey);
        return null;
    }

    /// <summary>Gets a policy by key without result type</summary>
    public IAsyncPolicy? GetPolicy(string policyKey)
    {
        if (_policies.TryGetValue(policyKey, out var policy))
        {
            return policy as IAsyncPolicy;
        }

        _logger.LogWarning("Policy not found for key: {PolicyKey}", policyKey);
        return null;
    }

    /// <summary>Initializes all resilience policies</summary>
    private void InitializePolicies()
    {
        _logger.LogInformation("Initializing resilience policies...");

        // Database Policy
        var dbPolicy = CreateDatabasePolicy();
        _policies[PolicyKeys.DatabasePolicy] = dbPolicy;
        _logger.LogInformation("Database policy initialized");

        // HTTP Policy
        var httpPolicy = CreateHttpPolicy();
        _policies[PolicyKeys.HttpPolicy] = httpPolicy;
        _logger.LogInformation("HTTP policy initialized");

        // RabbitMQ Policy
        var rmqPolicy = CreateRabbitMqPolicy();
        _policies[PolicyKeys.RabbitMqPolicy] = rmqPolicy;
        _logger.LogInformation("RabbitMQ policy initialized");

        // SMTP Policy
        var smtpPolicy = CreateSmtpPolicy();
        _policies[PolicyKeys.SmtpPolicy] = smtpPolicy;
        _logger.LogInformation("SMTP policy initialized");

        _logger.LogInformation("All resilience policies initialized successfully");
    }

    /// <summary>Creates database policy (retry + circuit breaker)</summary>
    private IAsyncPolicy CreateDatabasePolicy()
    {
        // Retry policy: 3 attempts with exponential backoff
        var retryPolicy = Policy
            .Handle<Exception>(ex => IsTransientDatabaseError(ex))
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(Math.Pow(2, attempt) * 100),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    _logger.LogWarning(
                        "Database operation retry {RetryCount}: waiting {WaitMs}ms",
                        retryCount,
                        timespan.TotalMilliseconds);
                });

        // Circuit breaker: opens after 5 failures, half-open after 20 seconds
        var circuitBreakerPolicy = Policy
            .Handle<Exception>(ex => IsTransientDatabaseError(ex))
            .CircuitBreakerAsync(
                5,
                TimeSpan.FromSeconds(20));

        // Timeout: 30 seconds
        var timeoutPolicy = Policy.TimeoutAsync(TimeSpan.FromSeconds(30));

        return Policy.WrapAsync(retryPolicy, circuitBreakerPolicy, timeoutPolicy);
    }

    /// <summary>Creates HTTP policy for external API calls</summary>
    private IAsyncPolicy CreateHttpPolicy()
    {
        // Retry policy: 2 attempts with exponential backoff
        var retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(
                retryCount: 2,
                sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(Math.Pow(2, attempt) * 200),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    _logger.LogWarning(
                        "HTTP call retry {RetryCount}: waiting {WaitMs}ms",
                        retryCount,
                        timespan.TotalMilliseconds);
                });

        // Circuit breaker: opens after 5 failures
        var circuitBreakerPolicy = Policy
            .Handle<HttpRequestException>()
            .CircuitBreakerAsync(
                5,
                TimeSpan.FromSeconds(30));

        // Timeout: 10 seconds
        var timeoutPolicy = Policy.TimeoutAsync(TimeSpan.FromSeconds(10));

        return Policy.WrapAsync(retryPolicy, circuitBreakerPolicy, timeoutPolicy);
    }

    /// <summary>Creates RabbitMQ policy</summary>
    private IAsyncPolicy CreateRabbitMqPolicy()
    {
        // Retry policy: 5 attempts with exponential backoff (connection issues are frequent)
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 5,
                sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(Math.Pow(2, attempt) * 150),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    _logger.LogWarning(
                        "RabbitMQ operation retry {RetryCount}: waiting {WaitMs}ms",
                        retryCount,
                        timespan.TotalMilliseconds);
                });

        // Circuit breaker: more lenient for RabbitMQ
        var circuitBreakerPolicy = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                8,
                TimeSpan.FromSeconds(45));

        // Timeout: 30 seconds
        var timeoutPolicy = Policy.TimeoutAsync(TimeSpan.FromSeconds(30));

        return Policy.WrapAsync(retryPolicy, circuitBreakerPolicy, timeoutPolicy);
    }

    /// <summary>Creates SMTP policy for email sending</summary>
    private IAsyncPolicy CreateSmtpPolicy()
    {
        // Retry policy: 3 attempts (email failures often transient)
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    _logger.LogWarning(
                        "SMTP operation retry {RetryCount}: waiting {WaitSeconds}s",
                        retryCount,
                        timespan.TotalSeconds);
                });

        // Timeout: 15 seconds
        var timeoutPolicy = Policy.TimeoutAsync(TimeSpan.FromSeconds(15));

        return Policy.WrapAsync(retryPolicy, timeoutPolicy);
    }

    /// <summary>Determines if an exception indicates a transient database error</summary>
    private static bool IsTransientDatabaseError(Exception ex)
    {
        if (ex is TimeoutException or InvalidOperationException)
            return true;

        var message = ex.Message.ToLowerInvariant();
        return message.Contains("timeout") ||
               message.Contains("connection") ||
               message.Contains("deadlock");
    }

    /// <summary>Determines if an HTTP result indicates a transient error</summary>
    private static bool IsTransientHttpError<T>(T result)
    {
        // This is a placeholder - implement based on your HTTP response type
        return false;
    }
}

/// <summary>
/// Generic resilience policy executor
/// </summary>
public class ResiliencePolicyExecutor
{
    private readonly IPolicyRegistry _policyRegistry;
    private readonly ILogger<ResiliencePolicyExecutor> _logger;

    public ResiliencePolicyExecutor(IPolicyRegistry policyRegistry, ILogger<ResiliencePolicyExecutor> logger)
    {
        _policyRegistry = policyRegistry;
        _logger = logger;
    }

    /// <summary>Executes code with resilience policies</summary>
    public async Task<TResult> ExecuteAsync<TResult>(
        string policyKey,
        Func<Task<TResult>> action,
        string actionName = "")
    {
        var policy = _policyRegistry.GetPolicy<TResult>(policyKey);
        if (policy == null)
        {
            _logger.LogWarning("Policy not found: {PolicyKey}, executing without resilience", policyKey);
            return await action();
        }

        try
        {
            _logger.LogDebug("Executing {ActionName} with policy {PolicyKey}", actionName, policyKey);
            return await policy.ExecuteAsync(action);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Resilience execution failed for {ActionName} with policy {PolicyKey}",
                actionName, policyKey);
            throw;
        }
    }

    /// <summary>Executes void code with resilience policies</summary>
    public async Task ExecuteAsync(
        string policyKey,
        Func<Task> action,
        string actionName = "")
    {
        var policy = _policyRegistry.GetPolicy(policyKey);
        if (policy == null)
        {
            _logger.LogWarning("Policy not found: {PolicyKey}, executing without resilience", policyKey);
            await action();
            return;
        }

        try
        {
            _logger.LogDebug("Executing {ActionName} with policy {PolicyKey}", actionName, policyKey);
            await policy.ExecuteAsync(action);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Resilience execution failed for {ActionName} with policy {PolicyKey}",
                actionName, policyKey);
            throw;
        }
    }
}
