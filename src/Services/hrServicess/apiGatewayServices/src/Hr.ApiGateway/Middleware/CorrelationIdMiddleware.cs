namespace Hr.ApiGateway.Middleware;

public sealed class CorrelationIdMiddleware(RequestDelegate next)
{
    public const string HeaderName = "X-Correlation-ID";

    public async Task Invoke(HttpContext context)
    {
        var correlationId = context.Request.Headers[HeaderName].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = Guid.NewGuid().ToString("N");
            context.Request.Headers[HeaderName] = correlationId;
        }

        context.Response.Headers[HeaderName] = correlationId;
        context.Items[HeaderName] = correlationId;

        await next(context);
    }
}
