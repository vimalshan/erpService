using System.Net;
using System.Text.Json;
using BusServices.Domain.Exceptions;
using FluentValidation;

namespace BusServices.API.Middleware;

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
            _logger.LogError(ex, "Unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        HttpStatusCode statusCode;
        string title;
        string[] errors;

        switch (exception)
        {
            case FluentValidation.ValidationException ve:
                statusCode = HttpStatusCode.BadRequest;
                title = "Validation Error";
                errors = ve.Errors.Select(e => e.ErrorMessage).ToArray();
                break;
            case Domain.Exceptions.DomainException de:
                statusCode = HttpStatusCode.BadRequest;
                title = "Domain Error";
                errors = new[] { de.Message };
                break;
            case System.Collections.Generic.KeyNotFoundException kne:
                statusCode = HttpStatusCode.NotFound;
                title = "Not Found";
                errors = new[] { kne.Message };
                break;
            case InvalidOperationException ioe:
                statusCode = HttpStatusCode.Conflict;
                title = "Conflict";
                errors = new[] { ioe.Message };
                break;
            default:
                statusCode = HttpStatusCode.InternalServerError;
                title = "Server Error";
                errors = new[] { "An unexpected error occurred." };
                break;
        }

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var response = new { title, status = (int)statusCode, errors };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
