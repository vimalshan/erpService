using System.Net;
using System.Text.Json;
using FluentValidation;
using WMTransactional.Domain.Exceptions;

namespace WMTransactional.API.Middleware;

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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            ValidationException validationEx => (
                HttpStatusCode.BadRequest,
                JsonSerializer.Serialize(new
                {
                    Error = "Validation Failed",
                    Details = validationEx.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                })),

            TransactionNotFoundException notFoundEx => (
                HttpStatusCode.NotFound,
                JsonSerializer.Serialize(new { Error = notFoundEx.Message })),

            InvalidTransactionStateException stateEx => (
                HttpStatusCode.Conflict,
                JsonSerializer.Serialize(new { Error = stateEx.Message })),

            System.Collections.Generic.KeyNotFoundException => (
                HttpStatusCode.NotFound,
                JsonSerializer.Serialize(new { Error = exception.Message })),

            _ => (
                HttpStatusCode.InternalServerError,
                JsonSerializer.Serialize(new { Error = "An unexpected error occurred.", Detail = exception.ToString() }))
        };

        _logger.LogError(exception, "Exception caught in middleware: {Message}", exception.Message);

        response.StatusCode = (int)statusCode;
        await response.WriteAsync(message);
    }
}
