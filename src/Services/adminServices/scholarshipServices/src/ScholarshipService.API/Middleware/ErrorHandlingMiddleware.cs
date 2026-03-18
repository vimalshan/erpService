using Microsoft.AspNetCore.Mvc;
using ScholarshipService.Application.Common;
using System.Net;
using System.Text.Json;

namespace ScholarshipService.API.Middleware;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
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
        logger.LogError(exception, "Unhandled exception: {Message}", exception.Message);

        var (statusCode, title) = exception switch
        {
            System.Collections.Generic.KeyNotFoundException => (HttpStatusCode.NotFound, "Resource Not Found"),
            FluentValidation.ValidationException ve => (HttpStatusCode.BadRequest, "Validation Failed"),
            InvalidOperationException => (HttpStatusCode.Conflict, "Operation Error"),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized"),
            _ => (HttpStatusCode.InternalServerError, "Internal Server Error")
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        var errors = exception is FluentValidation.ValidationException validationEx
            ? validationEx.Errors.Select(e => e.ErrorMessage).ToArray()
            : [exception.Message];

        var problem = new ProblemDetails
        {
            Title = title,
            Status = (int)statusCode,
            Detail = exception.Message,
            Instance = context.Request.Path,
            Extensions = { ["errors"] = errors, ["traceId"] = context.TraceIdentifier }
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
