using System.Net;
using System.Text.Json;
using FluentValidation;
using IntegrationService.Domain.Exceptions;

namespace IntegrationService.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
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
                JsonSerializer.Serialize(new
                {
                    Title = "Validation Error",
                    Errors = validationEx.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                })
            ),
            EntityNotFoundException => (HttpStatusCode.NotFound, JsonSerializer.Serialize(new { Title = "Not Found", Detail = exception.Message })),
            DomainException => (HttpStatusCode.BadRequest, JsonSerializer.Serialize(new { Title = "Domain Error", Detail = exception.Message })),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, JsonSerializer.Serialize(new { Title = "Unauthorized", Detail = "Access denied." })),
            _ => (HttpStatusCode.InternalServerError, JsonSerializer.Serialize(new { Title = "Server Error", Detail = "An unexpected error occurred." }))
        };

        if (statusCode == HttpStatusCode.InternalServerError)
            logger.LogError(exception, "Unhandled exception");
        else
            logger.LogWarning(exception, "Handled exception: {StatusCode}", statusCode);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsync(message);
    }
}
