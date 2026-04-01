using System.Net;
using System.Text.Json;
using FluentValidation;
using LetTransactionService.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LetTransactionService.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (LetNotFoundException ex)
        {
            logger.LogWarning(ex, "Resource not found");
            await WriteProblemAsync(context, HttpStatusCode.NotFound, "Not Found", ex.Message);
        }
        catch (LetDomainException ex)
        {
            logger.LogWarning(ex, "Domain rule violation");
            await WriteProblemAsync(context, HttpStatusCode.UnprocessableEntity, "Domain Rule Violation", ex.Message);
        }
        catch (ValidationException ex)
        {
            logger.LogWarning("Validation failed: {Errors}", ex.Errors.Select(e => e.ErrorMessage));
            var details = new
            {
                type = "https://tools.ietf.org/html/rfc7807",
                title = "Validation Failed",
                status = (int)HttpStatusCode.BadRequest,
                errors = ex.Errors.GroupBy(e => e.PropertyName)
                            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
            };
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/problem+json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(details));
        }
        catch (UnauthorizedAccessException ex)
        {
            logger.LogWarning(ex, "Unauthorized access");
            await WriteProblemAsync(context, HttpStatusCode.Unauthorized, "Unauthorized", "Authentication is required.");
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
            type = "https://tools.ietf.org/html/rfc7807",
            title,
            status = (int)statusCode,
            detail
        };
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
