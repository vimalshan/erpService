using System.Net;
using System.Text.Json;
using RecruitmentService.Domain.Exceptions;
using FluentValidation;

namespace RecruitmentService.API.Middleware;

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
            _logger.LogError(ex, "Unhandled exception for {Method} {Path}", context.Request.Method, context.Request.Path);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title, detail) = exception switch
        {
            ValidationException ve => (
                HttpStatusCode.BadRequest,
                "Validation Failed",
                string.Join("; ", ve.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"))),

            VacancyNotFoundException or ApplicationNotFoundException or ProspectNotFoundException => (
                HttpStatusCode.NotFound, "Not Found", exception.Message),

            VacancyClosedException => (
                HttpStatusCode.Conflict, "Vacancy Closed", exception.Message),

            DuplicateEmailException => (
                HttpStatusCode.Conflict, "Duplicate Email", exception.Message),

            DomainException => (
                HttpStatusCode.BadRequest, "Domain Error", exception.Message),

            UnauthorizedAccessException => (
                HttpStatusCode.Unauthorized, "Unauthorized", exception.Message),

            _ => (HttpStatusCode.InternalServerError, "Internal Server Error",
                "An unexpected error occurred. Please try again later.")
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        var problem = new
        {
            type = $"https://httpstatuses.com/{(int)statusCode}",
            title,
            status = (int)statusCode,
            detail,
            traceId = context.TraceIdentifier
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
