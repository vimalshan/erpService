namespace CertificateService.Middleware;

public class CorrelationIdMiddleware
{
    private readonly RequestDelegate _next;
    public CorrelationIdMiddleware(RequestDelegate next) => _next = next;
    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue("X-Correlation-Id", out var cid)) cid = Guid.NewGuid().ToString();
        context.Items["CorrelationId"] = cid.ToString(); context.Response.Headers["X-Correlation-Id"] = cid;
        await _next(context);
    }
}

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next; private readonly ILogger<GlobalExceptionMiddleware> _logger;
    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger) { _next = next; _logger = logger; }
    public async Task InvokeAsync(HttpContext context)
    {
        try { await _next(context); }
        catch (FluentValidation.ValidationException ex) { context.Response.StatusCode = 400; await context.Response.WriteAsJsonAsync(new { errors = ex.Errors.Select(e => e.ErrorMessage) }); }
        catch (System.Collections.Generic.KeyNotFoundException ex) { context.Response.StatusCode = 404; await context.Response.WriteAsJsonAsync(new { error = ex.Message }); }
        catch (Exception ex) { _logger.LogError(ex, "Unhandled exception"); context.Response.StatusCode = 500; await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred" }); }
    }
}
