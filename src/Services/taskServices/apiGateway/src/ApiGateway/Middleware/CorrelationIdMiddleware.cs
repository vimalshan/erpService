namespace ApiGateway.Middleware;

public class CorrelationIdMiddleware(RequestDelegate next)
{
    private const string CorrelationIdHeader = "X-Correlation-ID";

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.ContainsKey(CorrelationIdHeader))
        {
            var correlationId = Guid.NewGuid().ToString("N");
            context.Request.Headers[CorrelationIdHeader] = correlationId;
        }

        var id = context.Request.Headers[CorrelationIdHeader].ToString();

        // Set on response too so client can correlate
        context.Response.OnStarting(() =>
        {
            context.Response.Headers[CorrelationIdHeader] = id;
            return Task.CompletedTask;
        });

        // Add to log context
        using (Serilog.Context.LogContext.PushProperty("CorrelationId", id))
        {
            await next(context);
        }
    }
}
