using System.Net;
using System.Text.Json;
using FluentValidation;
using DispatchPlanning.Domain.Exceptions;

namespace DispatchPlanning.API.Middleware;

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
        _logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);

        var (statusCode, title, detail) = exception switch
        {
            DispatchPlanNotFoundException ex => (HttpStatusCode.NotFound, "Not Found", ex.Message),
            DispatchPlanItemNotFoundException ex => (HttpStatusCode.NotFound, "Item Not Found", ex.Message),
            DuplicateDispatchPlanItemException ex => (HttpStatusCode.Conflict, "Conflict", ex.Message),
            InvalidPlanTypeException ex => (HttpStatusCode.BadRequest, "Validation Error", ex.Message),
            ValidationException ex => (HttpStatusCode.BadRequest, "Validation Failed",
                string.Join("; ", ex.Errors.Select(e => e.ErrorMessage))),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized", "Authentication required."),
            _ => (HttpStatusCode.InternalServerError, "Server Error", "An unexpected error occurred.")
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        var problem = new
        {
            type = $"https://httpstatuses.io/{(int)statusCode}",
            title,
            status = (int)statusCode,
            detail,
            traceId = context.TraceIdentifier
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
