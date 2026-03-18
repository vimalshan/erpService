using System.Net;
using System.Text.Json;
using FluentValidation;
using EligibilityService.Domain.Exceptions;

namespace EligibilityService.API.Middleware;

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
        catch (ValidationException ex)
        {
            _logger.LogWarning("Validation error: {Errors}", ex.Message);
            await WriteErrorAsync(context, HttpStatusCode.BadRequest, "Validation failed.",
                ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (EligibilityNotFoundException ex)
        {
            _logger.LogWarning(ex.Message);
            await WriteErrorAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (DuplicateEligibilityException ex)
        {
            _logger.LogWarning(ex.Message);
            await WriteErrorAsync(context, HttpStatusCode.Conflict, ex.Message);
        }
        catch (System.Collections.Generic.KeyNotFoundException ex)
        {
            _logger.LogWarning(ex.Message);
            await WriteErrorAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception.");
            await WriteErrorAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }
    }

    private static Task WriteErrorAsync(
        HttpContext context,
        HttpStatusCode status,
        string message,
        IEnumerable<string>? details = null)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;

        var payload = new
        {
            status = (int)status,
            message,
            details = details?.ToArray()
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(payload));
    }
}
