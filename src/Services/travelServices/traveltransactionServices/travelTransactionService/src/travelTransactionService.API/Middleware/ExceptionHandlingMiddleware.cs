using System.Net;
using System.Text.Json;
using FluentValidation;

namespace travelTransactionService.API.Middleware;

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
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message) = exception switch
        {
            ValidationException validationException => (
                HttpStatusCode.BadRequest,
                new ErrorResponse("Validation Error",
                    validationException.Errors.Select(e => e.ErrorMessage).ToList())),

            System.Collections.Generic.KeyNotFoundException => (
                HttpStatusCode.NotFound,
                new ErrorResponse("Not Found", [exception.Message])),

            InvalidOperationException => (
                HttpStatusCode.BadRequest,
                new ErrorResponse("Bad Request", [exception.Message])),

            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                new ErrorResponse("Unauthorized", [exception.Message])),

            _ => (
                HttpStatusCode.InternalServerError,
                new ErrorResponse("Internal Server Error", ["An unexpected error occurred."]))
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var json = JsonSerializer.Serialize(message, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        await context.Response.WriteAsync(json);
    }
}

public record ErrorResponse(string Title, List<string> Errors);
