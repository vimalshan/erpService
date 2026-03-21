using System.Net;
using System.Text.Json;
using TourServices.Domain.Exceptions;

namespace TourServices.API.Middleware;

public sealed class ExceptionHandlingMiddleware
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
            _logger.LogError(ex, "Unhandled exception for {Method} {Path}",
                context.Request.Method, context.Request.Path);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var (status, title) = ex switch
        {
            TourNotFoundException => (HttpStatusCode.NotFound, "Tour Not Found"),
            TourFullyBookedException => (HttpStatusCode.Conflict, "Tour Fully Booked"),
            TourNotActiveException => (HttpStatusCode.UnprocessableEntity, "Tour Not Active"),
            FluentValidation.ValidationException => (HttpStatusCode.BadRequest, "Validation Failed"),
            _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred")
        };

        var errors = ex is FluentValidation.ValidationException ve
            ? ve.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }).ToArray<object>()
            : [new { Message = ex.Message }];

        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/json";

        var problem = new
        {
            title,
            status = (int)status,
            detail = ex.Message,
            errors
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(problem, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }
}
