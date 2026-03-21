using System.Net;
using System.Text.Json;
using FluentValidation;

namespace CustomerService.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            ValidationException validationEx => (
                HttpStatusCode.BadRequest,
                (object)new
                {
                    Title = "Validation Error",
                    Errors = validationEx.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                }),
            System.Collections.Generic.KeyNotFoundException => (
                HttpStatusCode.NotFound,
                (object)new { Title = "Not Found", Detail = exception.Message }),
            InvalidOperationException => (
                HttpStatusCode.Conflict,
                (object)new { Title = "Conflict", Detail = exception.Message }),
            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                (object)new { Title = "Unauthorized", Detail = exception.Message }),
            _ => (
                HttpStatusCode.InternalServerError,
                (object)new { Title = "Server Error", Detail = "An unexpected error occurred." })
        };

        if (statusCode == HttpStatusCode.InternalServerError)
            _logger.LogError(exception, "Unhandled exception");
        else
            _logger.LogWarning(exception, "Handled exception: {StatusCode}", statusCode);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var json = JsonSerializer.Serialize(message, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        await context.Response.WriteAsync(json);
    }
}
