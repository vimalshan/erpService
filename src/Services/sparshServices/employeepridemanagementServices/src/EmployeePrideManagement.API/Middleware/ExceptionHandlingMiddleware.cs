using System.Net;
using System.Text.Json;
using EmployeePrideManagement.Domain.Exceptions;
using FluentValidation;

namespace EmployeePrideManagement.API.Middleware;

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
                new ErrorResponse
                {
                    Message = "Validation failed.",
                    Errors = validationEx.Errors.Select(e => e.ErrorMessage).ToList()
                }),
            PrideMomentNotFoundException => (
                HttpStatusCode.NotFound,
                new ErrorResponse { Message = exception.Message }),
            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                new ErrorResponse { Message = "Unauthorized access." }),
            _ => (
                HttpStatusCode.InternalServerError,
                new ErrorResponse { Message = "An unexpected error occurred." })
        };

        if (statusCode == HttpStatusCode.InternalServerError)
            _logger.LogError(exception, "Unhandled exception occurred.");
        else
            _logger.LogWarning("Handled exception: {Message}", exception.Message);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var json = JsonSerializer.Serialize(message, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public List<string>? Errors { get; set; }
}
