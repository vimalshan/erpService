namespace Hr.ApiGateway.Middleware;

public sealed class BulkheadIsolationMiddleware(RequestDelegate next, IConfiguration configuration)
{
    private static readonly SemaphoreSlim Guard = new(
        initialCount: 200,
        maxCount: 200);

    private readonly int _waitTimeoutMs = configuration.GetValue<int?>("Bulkhead:WaitTimeoutMs") ?? 500;

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/health") || context.Request.Path.StartsWithSegments("/metrics"))
        {
            await next(context);
            return;
        }

        if (!await Guard.WaitAsync(_waitTimeoutMs, context.RequestAborted))
        {
            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            await context.Response.WriteAsync("Gateway bulkhead limit reached.", context.RequestAborted);
            return;
        }

        try
        {
            await next(context);
        }
        finally
        {
            Guard.Release();
        }
    }
}
