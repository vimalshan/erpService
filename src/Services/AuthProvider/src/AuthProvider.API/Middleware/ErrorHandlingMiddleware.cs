using System.Net;
using System.Text.Json;
using KeyNotFoundException = System.Collections.Generic.KeyNotFoundException;

namespace AuthProvider.API.Middleware;

/// <summary>
/// Common Error and Exception Handling middleware.
/// Catches all unhandled exceptions and returns a structured JSON error response.
/// Prevents leaking stack traces to clients (security best practice).
/// </summary>
public sealed class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ErrorHandlingMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlingMiddleware> logger,
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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var correlationId = context.Items["CorrelationId"]?.ToString() ?? Guid.NewGuid().ToString();
        var (statusCode, title) = exception switch
        {
            FluentValidation.ValidationException validationEx =>
                (HttpStatusCode.BadRequest, "Validation Error"),
            UnauthorizedAccessException =>
                (HttpStatusCode.Unauthorized, "Unauthorized"),
            KeyNotFoundException =>
                (HttpStatusCode.NotFound, "Not Found"),
            InvalidOperationException =>
                (HttpStatusCode.Conflict, "Conflict"),
            NotSupportedException =>
                (HttpStatusCode.BadRequest, "Bad Request"),
            _ => (HttpStatusCode.InternalServerError, "Internal Server Error")
        };

        _logger.LogError(exception,
            "Unhandled exception [{CorrelationId}] {ExceptionType}: {Message}",
            correlationId, exception.GetType().Name, exception.Message);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var errorResponse = new ErrorResponse
        {
            TraceId = correlationId,
            Title = title,
            Status = (int)statusCode,
            Detail = _env.IsDevelopment() ? exception.ToString() : exception.Message,
            Errors = exception is FluentValidation.ValidationException ve
                ? ve.Errors.GroupBy(e => e.PropertyName)
                          .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
                : null
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        }));
    }
}

public sealed class ErrorResponse
{
    public string TraceId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Status { get; set; }
    public string Detail { get; set; } = string.Empty;
    public Dictionary<string, string[]>? Errors { get; set; }
}
