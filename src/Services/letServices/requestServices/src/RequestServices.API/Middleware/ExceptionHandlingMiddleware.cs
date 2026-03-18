using System.Net;
using System.Text.Json;
using FluentValidation;
using RequestServices.Domain.Exceptions;

namespace RequestServices.API.Middleware;

/// <summary>Global exception-handling middleware — maps domain/validation exceptions to RFC 7807 problem details.</summary>
public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (RequestNotFoundException ex)
        {
            logger.LogWarning(ex, "Resource not found");
            await WriteProblemAsync(context, HttpStatusCode.NotFound, "Not Found", ex.Message);
        }
        catch (RequestDomainException ex)
        {
            logger.LogWarning(ex, "Domain rule violation");
            await WriteProblemAsync(context, HttpStatusCode.UnprocessableEntity, "Domain Rule Violation", ex.Message);
        }
        catch (ValidationException ex)
        {
            logger.LogWarning("Validation failed: {Errors}", ex.Errors.Select(e => e.ErrorMessage));
            var details = new
            {
                type    = "https://tools.ietf.org/html/rfc7807",
                title   = "Validation Failed",
                status  = (int)HttpStatusCode.BadRequest,
                errors  = ex.Errors.GroupBy(e => e.PropertyName)
                            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
            };
            context.Response.StatusCode  = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(details));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            await WriteProblemAsync(context, HttpStatusCode.InternalServerError,
                "Internal Server Error", "An unexpected error occurred. Please try again later.");
        }
    }

    private static async Task WriteProblemAsync(
        HttpContext context, HttpStatusCode statusCode, string title, string detail)
    {
        var problem = new
        {
            type   = "https://tools.ietf.org/html/rfc7807",
            title,
            status = (int)statusCode,
            detail
        };
        context.Response.StatusCode  = (int)statusCode;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}

/// <summary>Middleware that logs incoming HTTP request details.</summary>
public class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        logger.LogInformation("HTTP {Method} {Path} from {Remote}",
            context.Request.Method,
            context.Request.Path,
            context.Connection.RemoteIpAddress);

        await next(context);

        logger.LogInformation("HTTP {Status} for {Method} {Path}",
            context.Response.StatusCode,
            context.Request.Method,
            context.Request.Path);
    }
}
