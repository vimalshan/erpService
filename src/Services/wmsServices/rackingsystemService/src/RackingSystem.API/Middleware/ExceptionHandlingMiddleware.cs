using System.Net;
using System.Text.Json;
using FluentValidation;
using RackingSystem.Domain.Exceptions;

namespace RackingSystem.API.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next   = next;
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
        _logger.LogError(exception, "Unhandled exception");

        var (statusCode, title, detail) = exception switch
        {
            NotFoundException e      => (HttpStatusCode.NotFound, "Not Found", e.Message),
            ValidationException e    => (HttpStatusCode.BadRequest, "Validation Error",
                string.Join("; ", e.Errors.Select(x => x.ErrorMessage))),
            DomainException e        => (HttpStatusCode.UnprocessableEntity, "Domain Error", e.Message),
            InvalidOperationException e => (HttpStatusCode.Conflict, "Conflict", e.Message),
            _                        => (HttpStatusCode.InternalServerError, "Internal Server Error",
                "An unexpected error occurred.")
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode  = (int)statusCode;

        var problem = new
        {
            type     = $"https://httpstatuses.com/{(int)statusCode}",
            title,
            status   = (int)statusCode,
            detail,
            instance = context.Request.Path.Value,
            traceId  = context.TraceIdentifier
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
