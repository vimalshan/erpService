using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace ReimbursementService.API.Middleware;

public sealed class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        var (status, title, errors) = exception switch
        {
            ValidationException ve => (
                HttpStatusCode.BadRequest,
                "Validation failed",
                ve.Errors.Select(e => e.ErrorMessage).ToArray()),
            System.Collections.Generic.KeyNotFoundException => (
                HttpStatusCode.NotFound,
                exception.Message,
                Array.Empty<string>()),
            InvalidOperationException => (
                HttpStatusCode.UnprocessableEntity,
                exception.Message,
                Array.Empty<string>()),
            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                "Unauthorized",
                Array.Empty<string>()),
            _ => (
                HttpStatusCode.InternalServerError,
                "An unexpected error occurred.",
                Array.Empty<string>())
        };

        context.Response.StatusCode = (int)status;

        var problem = new ProblemDetails
        {
            Status = (int)status,
            Title = title,
            Extensions = { ["errors"] = errors }
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
