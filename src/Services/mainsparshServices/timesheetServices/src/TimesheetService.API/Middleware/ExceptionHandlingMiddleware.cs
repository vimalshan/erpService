using System.Net;
using System.Text.Json;
using FluentValidation;
using TimesheetService.Domain.Exceptions;

namespace TimesheetService.API.Middleware;

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
        var (statusCode, title, errors) = exception switch
        {
            TimesheetNotFoundException nfe =>
                (HttpStatusCode.NotFound, nfe.Message, (IEnumerable<string>?)null),

            TimesheetDomainException de =>
                (HttpStatusCode.UnprocessableEntity, de.Message, null),

            ValidationException ve =>
                (HttpStatusCode.BadRequest, "One or more validation errors occurred.",
                 ve.Errors.Select(e => e.ErrorMessage)),

            UnauthorizedAccessException =>
                (HttpStatusCode.Unauthorized, "Unauthorized.", null),

            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.", null)
        };

        if (exception is not (TimesheetNotFoundException or TimesheetDomainException or ValidationException))
            _logger.LogError(exception, "Unhandled exception");

        context.Response.ContentType = "application/json";
        context.Response.StatusCode  = (int)statusCode;

        var body = new ProblemResponse(title, (int)statusCode, errors?.ToList());
        await context.Response.WriteAsync(JsonSerializer.Serialize(body, CamelCaseOptions));
    }

    private static readonly JsonSerializerOptions CamelCaseOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private sealed record ProblemResponse(string Title, int Status, List<string>? Errors);
}
