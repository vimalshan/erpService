using CalendarService.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CalendarService.API.Middleware;

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
            logger.LogError(ex, "Unhandled exception for {Method} {Path}", context.Request.Method, context.Request.Path);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (status, title) = exception switch
        {
            CalendarNotFoundException or ShiftNotFoundException
                or HolidayNotFoundException or PatternNotFoundException
                => (StatusCodes.Status404NotFound, "Resource Not Found"),

            DuplicateCalendarNameException or DuplicateShiftCodeException
                => (StatusCodes.Status409Conflict, "Duplicate Resource"),

            ValidationException => (StatusCodes.Status400BadRequest, "Validation Failed"),

            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
        };

        var details = exception is ValidationException ve
            ? (object)new { errors = ve.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) }
            : exception.Message;

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = exception.Message,
            Extensions = { ["errors"] = details }
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = status;

        return context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
