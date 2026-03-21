using System.Net;
using System.Text.Json;
using TourPlanService.Domain.Exceptions;

namespace TourPlanService.API.Middleware;

public sealed class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        // Let HotChocolate handle its own errors for the /graphql path
        if (context.Request.Path.StartsWithSegments("/graphql"))
        {
            await next(context);
            return;
        }

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
        context.Response.ContentType = "application/json";

        var (statusCode, errors) = exception switch
        {
            ValidationException ve => (HttpStatusCode.BadRequest,
                new ProblemDetails { Title = "Validation Error", Errors = ve.Errors }),
            NotFoundException nfe => (HttpStatusCode.NotFound,
                new ProblemDetails { Title = "Not Found", Detail = nfe.Message }),
            DomainException de => (HttpStatusCode.UnprocessableEntity,
                new ProblemDetails { Title = "Business Rule Violation", Detail = de.Message }),
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized,
                new ProblemDetails { Title = "Unauthorized" }),
            _ => (HttpStatusCode.InternalServerError,
                new ProblemDetails { Title = "Internal Server Error", Detail = "An unexpected error occurred." })
        };

        context.Response.StatusCode = (int)statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(errors,
            new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }));
    }

    private sealed class ProblemDetails
    {
        public string Title { get; set; } = default!;
        public string? Detail { get; set; }
        public IDictionary<string, string[]>? Errors { get; set; }
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app) =>
        app.UseMiddleware<ExceptionHandlingMiddleware>();
}
