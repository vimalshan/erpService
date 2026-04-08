namespace OrderScheduleService.API.Middleware;

using System.Net;
using System.Text.Json;

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
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var statusCode = HttpStatusCode.InternalServerError;

        if (exception is InvalidOperationException)
            statusCode = HttpStatusCode.BadRequest;
        else if (exception is UnauthorizedAccessException)
            statusCode = HttpStatusCode.Unauthorized;
        else if (exception is KeyNotFoundException)
            statusCode = HttpStatusCode.NotFound;

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            error = exception.Message,
            statusCode = (int)statusCode,
            timestamp = DateTime.UtcNow
        };

        return context.Response.WriteAsJsonAsync(response);
    }
}

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation($"Incoming request: {context.Request.Method} {context.Request.Path}");

        // Skip response body buffering for GraphQL — HotChocolate manages its own response stream
        if (context.Request.Path.StartsWithSegments("/graphql"))
        {
            await _next(context);
            _logger.LogInformation($"Outgoing response: {context.Response.StatusCode}");
            return;
        }

        var originalBodyStream = context.Response.Body;
        using (var responseBody = new MemoryStream())
        {
            context.Response.Body = responseBody;

            await _next(context);

            _logger.LogInformation($"Outgoing response: {context.Response.StatusCode}");

            responseBody.Seek(0, SeekOrigin.Begin);
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
