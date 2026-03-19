using FluentValidation;
using PurchaseSalesService.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace PurchaseSalesService.API.Middleware;

public sealed class ExceptionHandlingMiddleware
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
            _logger.LogError(ex, "Unhandled exception for {Method} {Path}", context.Request.Method, context.Request.Path);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, message, errors) = exception switch
        {
            PurchaseNotFoundException or SaleNotFoundException =>
                (HttpStatusCode.NotFound, exception.Message, (IEnumerable<string>?)null),
            ValidationException ve =>
                (HttpStatusCode.BadRequest, "Validation failed.",
                 ve.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}")),
            InvalidOperationException ioe =>
                (HttpStatusCode.Conflict, ioe.Message, null),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.", null)
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var payload = JsonSerializer.Serialize(new
        {
            status = (int)statusCode,
            message,
            errors
        });

        return context.Response.WriteAsync(payload);
    }
}
