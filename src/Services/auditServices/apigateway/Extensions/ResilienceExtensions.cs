using Polly;
using Polly.Bulkhead;
using Polly.CircuitBreaker;
using Polly.Extensions.Http;
using Polly.Timeout;
using Yarp.ReverseProxy.Transforms;

namespace ApiGateway.Extensions;

public static class ResilienceExtensions
{
    public static IServiceCollection AddGatewayReverseProxy(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var cbSection      = configuration.GetSection("Resilience:CircuitBreaker");
        var retrySection   = configuration.GetSection("Resilience:Retry");
        var timeoutSection = configuration.GetSection("Resilience:Timeout");
        var bhSection      = configuration.GetSection("Resilience:Bulkhead");

        double failureRatio      = cbSection.GetValue<double>("FailureRatio", 0.5);
        int    samplingDuration  = cbSection.GetValue<int>("SamplingDurationSeconds", 30);
        int    minThroughput     = cbSection.GetValue<int>("MinimumThroughput", 10);
        int    breakDuration     = cbSection.GetValue<int>("BreakDurationSeconds", 30);

        int    maxRetries        = retrySection.GetValue<int>("MaxRetryAttempts", 3);
        int    retryDelay        = retrySection.GetValue<int>("DelaySeconds", 1);
        bool   exponentialBackoff = retrySection.GetValue<bool>("UseExponentialBackoff", true);

        int    timeoutSeconds    = timeoutSection.GetValue<int>("TimeoutSeconds", 30);

        int    maxParallelization = bhSection.GetValue<int>("MaxParallelization", 100);
        int    maxQueuing         = bhSection.GetValue<int>("MaxQueuingActions", 50);

        // Circuit Breaker policy
        var circuitBreakerPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .AdvancedCircuitBreakerAsync(
                failureThreshold: failureRatio,
                samplingDuration: TimeSpan.FromSeconds(samplingDuration),
                minimumThroughput: minThroughput,
                durationOfBreak: TimeSpan.FromSeconds(breakDuration),
                onBreak: (outcome, state, duration, ctx) =>
                {
                    var logger = GetLogger(ctx);
                    logger?.LogWarning(
                        "Circuit breaker OPENED for {OperationKey} | Duration: {Duration}s | " +
                        "Reason: {Reason}",
                        ctx.OperationKey, duration.TotalSeconds, outcome.Exception?.Message);
                },
                onReset: ctx =>
                {
                    var logger = GetLogger(ctx);
                    logger?.LogInformation(
                        "Circuit breaker RESET for {OperationKey}", ctx.OperationKey);
                },
                onHalfOpen: () => { /* logged on next attempt */ });

        // Retry policy with exponential backoff
        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(
                retryCount: maxRetries,
                sleepDurationProvider: attempt => exponentialBackoff
                    ? TimeSpan.FromSeconds(retryDelay * Math.Pow(2, attempt - 1))
                    : TimeSpan.FromSeconds(retryDelay),
                onRetry: (outcome, timespan, attempt, ctx) =>
                {
                    var logger = GetLogger(ctx);
                    logger?.LogWarning(
                        "Retry attempt {Attempt}/{Max} for {OperationKey} | " +
                        "Wait: {Wait}ms | Reason: {Reason}",
                        attempt, maxRetries, ctx.OperationKey,
                        timespan.TotalMilliseconds,
                        outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString());
                });

        // Timeout policy
        var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(
            TimeSpan.FromSeconds(timeoutSeconds),
            TimeoutStrategy.Optimistic,
            onTimeoutAsync: (ctx, ts, _, _) =>
            {
                var logger = GetLogger(ctx);
                logger?.LogWarning(
                    "Request timed out for {OperationKey} after {Timeout}s",
                    ctx.OperationKey, ts.TotalSeconds);
                return Task.CompletedTask;
            });

        // Bulkhead policy
        var bulkheadPolicy = Policy.BulkheadAsync<HttpResponseMessage>(
            maxParallelization: maxParallelization,
            maxQueuingActions: maxQueuing,
            onBulkheadRejectedAsync: ctx =>
            {
                var logger = GetLogger(ctx);
                logger?.LogWarning(
                    "Bulkhead rejected request for {OperationKey} — too many concurrent requests",
                    ctx.OperationKey);
                return Task.CompletedTask;
            });

        // Combined policy: bulkhead → retry → circuit breaker → timeout
        var combinedPolicy = Policy.WrapAsync(bulkheadPolicy, retryPolicy, circuitBreakerPolicy, timeoutPolicy);

        services
            .AddReverseProxy()
            .LoadFromConfig(configuration.GetSection("ReverseProxy"))
            .AddTransforms(ctx =>
            {
                // Forward correlation ID downstream
                ctx.AddRequestTransform(async reqCtx =>
                {
                    if (reqCtx.HttpContext.Items["CorrelationId"] is string correlationId)
                    {
                        reqCtx.ProxyRequest.Headers.TryAddWithoutValidation(
                            "X-Correlation-Id", correlationId);
                    }
                    await Task.CompletedTask;
                });
            });

        // Register the combined Polly policy for YARP's forwarding HttpClient
        services.AddHttpClient("Yarp.ReverseProxy")
            .AddPolicyHandler(combinedPolicy);

        return services;
    }

    private static ILogger? GetLogger(Context ctx)
    {
        if (ctx.TryGetValue("IServiceProvider", out var sp) && sp is IServiceProvider provider)
            return provider.GetService<ILoggerFactory>()?.CreateLogger("ApiGateway.Resilience");
        return null;
    }
}
