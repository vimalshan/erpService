using System.Net;
using System.Text.Json;
using FluentValidation;
using ClubMembershipService.Domain.Exceptions;

namespace ClubMembershipService.API.Middleware;

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
            _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (statusCode, title, detail) = exception switch
        {
            ValidationException ve => (HttpStatusCode.BadRequest, "Validation Error",
                string.Join("; ", ve.Errors.Select(e => e.ErrorMessage))),
            ClubNotFoundException or MembershipNotFoundException or ActivityNotFoundException =>
                (HttpStatusCode.NotFound, "Not Found", exception.Message),
            DuplicateMembershipException => (HttpStatusCode.Conflict, "Conflict", exception.Message),
            DomainException => (HttpStatusCode.UnprocessableEntity, "Domain Error", exception.Message),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized", exception.Message),
            _ => (HttpStatusCode.InternalServerError, "Internal Server Error", "An unexpected error occurred.")
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
