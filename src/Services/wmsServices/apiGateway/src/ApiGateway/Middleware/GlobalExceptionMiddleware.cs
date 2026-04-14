using System.Net;

namespace ApiGateway.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault() ?? "N/A";
            _logger.LogError(ex, "[{CorrelationId}] Unhandled exception in gateway", correlationId);

            context.Response.StatusCode = (int)HttpStatusCode.BadGateway;
            context.Response.ContentType = "application/json";

            var response = new
            {
                statusCode = 502,
                message = "An error occurred in the API Gateway. Please try again later.",
                correlationId
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
