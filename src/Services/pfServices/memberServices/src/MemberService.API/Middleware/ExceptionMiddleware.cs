using System.Net;
using System.Text.Json;
using FluentValidation;
using MemberService.Domain.Exceptions;

namespace MemberService.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
        var (statusCode, title, errors) = exception switch
        {
            MemberDomainException ex => (HttpStatusCode.BadRequest, "Domain Error",
                new[] { ex.Message }),
            ValidationException ex => (HttpStatusCode.UnprocessableEntity, "Validation Error",
                ex.Errors.Select(e => e.ErrorMessage).ToArray()),
            System.Collections.Generic.KeyNotFoundException ex => (HttpStatusCode.NotFound, "Not Found",
                new[] { ex.Message }),
            UnauthorizedAccessException ex => (HttpStatusCode.Unauthorized, "Unauthorized",
                new[] { ex.Message }),
            _ => (HttpStatusCode.InternalServerError, "Internal Server Error",
                new[] { "An unexpected error occurred." })
        };

        if (statusCode == HttpStatusCode.InternalServerError)
            _logger.LogError(exception, "Unhandled exception");

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var response = JsonSerializer.Serialize(new
        {
            status = (int)statusCode,
            title,
            errors,
            traceId = context.TraceIdentifier
        });

        await context.Response.WriteAsync(response);
    }
}
