using System.Net;
using System.Text.Json;

namespace ApiGateway.Middleware;

public sealed class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var correlationId = context.Items["CorrelationId"]?.ToString() ?? "-";
            logger.LogError(ex, "[{CorrelationId}] Unhandled exception on {Method} {Path}",
                correlationId, context.Request.Method, context.Request.Path);

            context.Response.StatusCode = (int)HttpStatusCode.BadGateway;
            context.Response.ContentType = "application/json";

            var problem = new
            {
                type = "https://httpstatuses.com/502",
                title = "Bad Gateway",
                status = 502,
                detail = "An error occurred while processing your request through the gateway.",
                correlationId
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }
}
