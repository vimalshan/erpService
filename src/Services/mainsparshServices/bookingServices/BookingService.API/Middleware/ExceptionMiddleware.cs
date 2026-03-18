using FluentValidation;
using BookingService.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BookingService.API.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception for {Method} {Path}", context.Request.Method, context.Request.Path);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        (HttpStatusCode status, string title, string detail) = exception switch
        {
            ValidationException ve => (HttpStatusCode.BadRequest, "Validation Failed",
                string.Join("; ", ve.Errors.Select(e => e.ErrorMessage))),
            BookingDomainException de => (HttpStatusCode.UnprocessableEntity, "Domain Rule Violation", de.Message),
            System.Collections.Generic.KeyNotFoundException kne => (HttpStatusCode.NotFound, "Not Found", kne.Message),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized", "Authentication required."),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.", "Please contact support.")
        };

        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Status = (int)status,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        await context.Response.WriteAsJsonAsync(problem);
    }
}
