namespace LoanApiGateway.Middleware;

/// <summary>
/// Returns a structured JSON error response for unhandled exceptions
/// instead of the default developer exception page.
/// </summary>
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public GlobalExceptionMiddleware(
        RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger,
        IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var correlationId = context.Items.TryGetValue(CorrelationIdMiddleware.HeaderName, out var c)
                ? c?.ToString() : "N/A";

            _logger.LogError(ex,
                "[{CorrelationId}] Unhandled exception on {Method} {Path}",
                correlationId,
                context.Request.Method,
                context.Request.Path);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            string? errorDetail = _env.IsDevelopment() ? ex.Message : null;
            var response = new { error = "An unexpected error occurred.", detail = errorDetail, correlationId };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}

public static class GlobalExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        => app.UseMiddleware<GlobalExceptionMiddleware>();
}
