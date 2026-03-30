using Polly;
using Polly.CircuitBreaker;
using Polly.Timeout;

namespace Hr.ApiGateway.Middleware;

public sealed class GatewayResilienceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GatewayResilienceMiddleware> _logger;
    private readonly ResiliencePipeline _pipeline;

    public GatewayResilienceMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<GatewayResilienceMiddleware> logger)
    {
        _next = next;
        _logger = logger;

        var timeoutSeconds = configuration.GetValue<int?>("Resilience:TimeoutSeconds") ?? 30;
        var retryCount = configuration.GetValue<int?>("Resilience:RetryCount") ?? 2;
        var failuresBeforeBreak = configuration.GetValue<int?>("Resilience:CircuitBreakerFailures") ?? 8;
        var breakDurationSeconds = configuration.GetValue<int?>("Resilience:CircuitBreakerDurationSeconds") ?? 30;

        _pipeline = new ResiliencePipelineBuilder()
            .AddTimeout(TimeSpan.FromSeconds(timeoutSeconds))
            .AddRetry(new Polly.Retry.RetryStrategyOptions
            {
                MaxRetryAttempts = retryCount,
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                ShouldHandle = new PredicateBuilder()
                    .Handle<TimeoutRejectedException>()
                    .Handle<BadHttpRequestException>()
            })
            .AddCircuitBreaker(new Polly.CircuitBreaker.CircuitBreakerStrategyOptions
            {
                FailureRatio = 0.5,
                MinimumThroughput = failuresBeforeBreak,
                SamplingDuration = TimeSpan.FromSeconds(30),
                BreakDuration = TimeSpan.FromSeconds(breakDurationSeconds)
            })
            .Build();
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/health") || context.Request.Path.StartsWithSegments("/metrics"))
        {
            await _next(context);
            return;
        }

        try
        {
            await _pipeline.ExecuteAsync(async token =>
            {
                context.RequestAborted.ThrowIfCancellationRequested();
                await _next(context);

                if (context.Response.StatusCode >= 500)
                {
                    throw new BadHttpRequestException($"Downstream call failed with status {context.Response.StatusCode}.");
                }
            }, context.RequestAborted);
        }
        catch (BrokenCircuitException)
        {
            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            await context.Response.WriteAsync("Gateway circuit breaker is open.", context.RequestAborted);
        }
        catch (TimeoutRejectedException)
        {
            context.Response.StatusCode = StatusCodes.Status504GatewayTimeout;
            await context.Response.WriteAsync("Gateway timeout while processing downstream request.", context.RequestAborted);
        }
        catch (Exception ex) when (context.Response.HasStarted is false)
        {
            _logger.LogError(ex, "Gateway resilience middleware failed.");
            context.Response.StatusCode = StatusCodes.Status502BadGateway;
            await context.Response.WriteAsync("Gateway failed to process the downstream request.", context.RequestAborted);
        }
    }
}
