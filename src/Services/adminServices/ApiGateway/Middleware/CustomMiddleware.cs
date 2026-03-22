using System.Diagnostics;
using System.Text.Json;

namespace ApiGateway.Middleware;

/// <summary>
/// Request/response logging middleware with correlation IDs and timing
/// </summary>
public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

    public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Add correlation ID if not present
        var correlationId = context.Request.Headers["X-Correlation-ID"].FirstOrDefault() 
            ?? Guid.NewGuid().ToString();
        context.Items["CorrelationId"] = correlationId;
        context.Response.Headers.Append("X-Correlation-ID", correlationId);

        var stopwatch = Stopwatch.StartNew();

        // Log request
        _logger.LogInformation(
            "Request [{CorrelationId}]: {Method} {Path} from {RemoteIP}",
            correlationId, context.Request.Method, context.Request.Path, context.Connection.RemoteIpAddress);

        // Skip response body capture for static content (Swagger UI, etc.)
        var path = context.Request.Path.Value?.ToLowerInvariant() ?? "";
        bool isStaticContent = path.StartsWith("/swagger") || path.StartsWith("/.well-known") || 
                               path.EndsWith(".html") || path.EndsWith(".css") || path.EndsWith(".js") || 
                               path.EndsWith(".json") || path.EndsWith(".png") || path.EndsWith(".svg");

        if (isStaticContent)
        {
            var stopwatch2 = Stopwatch.StartNew();
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Request [{CorrelationId}]: Exception occurred - {Message}",
                    correlationId, ex.Message);
                throw;
            }
            stopwatch2.Stop();

            _logger.LogInformation(
                "Response [{CorrelationId}]: {StatusCode} {Method} {Path} completed in {ElapsedMs}ms",
                correlationId, context.Response.StatusCode, context.Request.Method,
                context.Request.Path, stopwatch2.ElapsedMilliseconds);
        }
        else
        {
            // Capture response body for non-static requests
            var originalBodyStream = context.Response.Body;
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex,
                        "Request [{CorrelationId}]: Exception occurred - {Message}",
                        correlationId, ex.Message);
                    throw;
                }

                stopwatch.Stop();

                // Log response
                _logger.LogInformation(
                    "Response [{CorrelationId}]: {StatusCode} {Method} {Path} completed in {ElapsedMs}ms",
                    correlationId, context.Response.StatusCode, context.Request.Method,
                    context.Request.Path, stopwatch.ElapsedMilliseconds);

                // Reset position and copy response to original stream
                responseBody.Position = 0;
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
    }
}

/// <summary>
/// Error handling middleware for consistent error responses
/// </summary>
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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
            _logger.LogError(ex, "Unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ErrorResponse
        {
            Message = "An internal server error occurred",
            StatusCode = context.Response.StatusCode = StatusCodes.Status500InternalServerError,
            Timestamp = DateTime.UtcNow,
            Path = context.Request.Path
        };

        if (exception is HttpRequestException httpEx)
        {
            response.StatusCode = (int?)httpEx.StatusCode ?? StatusCodes.Status502BadGateway;
            context.Response.StatusCode = response.StatusCode;
            response.Message = "Service is unavailable";
        }

        return context.Response.WriteAsJsonAsync(response);
    }

    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public DateTime Timestamp { get; set; }
        public string Path { get; set; } = string.Empty;
    }
}

/// <summary>
/// Security headers middleware
/// </summary>
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip strict CSP for Swagger UI (it uses inline scripts/styles)
        var path = context.Request.Path.Value?.ToLowerInvariant() ?? "";
        if (path.StartsWith("/swagger"))
        {
            await _next(context);
            return;
        }

        // Security headers
        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
        context.Response.Headers.Append("X-Frame-Options", "DENY");
        context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
        context.Response.Headers.Append("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
        context.Response.Headers.Append("Content-Security-Policy", "default-src 'self'");
        context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");

        await _next(context);
    }
}

/// <summary>
/// Request validation middleware for common validations
/// </summary>
public class RequestValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestValidationMiddleware> _logger;

    public RequestValidationMiddleware(RequestDelegate next, ILogger<RequestValidationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Validate request size
        if (context.Request.ContentLength.HasValue && context.Request.ContentLength > 10_485_760) // 10MB
        {
            _logger.LogWarning("Request exceeds maximum allowed size");
            context.Response.StatusCode = StatusCodes.Status413PayloadTooLarge;
            await context.Response.WriteAsJsonAsync(new { error = "Request body too large" });
            return;
        }

        // Validate required headers for mutation requests
        if (IsMutationRequest(context.Request.Method))
        {
            if (!context.Request.Headers.ContainsKey("Content-Type"))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new { error = "Content-Type header is required" });
                return;
            }
        }

        await _next(context);
    }

    private static bool IsMutationRequest(string method) =>
        method == "POST" || method == "PUT" || method == "DELETE" || method == "PATCH";
}
