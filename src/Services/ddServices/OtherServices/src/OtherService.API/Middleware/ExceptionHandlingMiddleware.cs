using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using FluentValidation;
using OtherService.Domain.Exceptions;

namespace OtherService.API.Middleware;

/// <summary>
/// Global exception handler – maps domain / validation exceptions to JSON problem details.
/// </summary>
public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (ValidationException ex)
        {
            await WriteAsync(context, HttpStatusCode.BadRequest,
                "Validation failed.",
                ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Domain exception");
            await WriteAsync(context, HttpStatusCode.UnprocessableEntity, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }
    }

    private static Task WriteAsync(
        HttpContext context,
        HttpStatusCode status,
        string title,
        IEnumerable<string>? errors = null)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode  = (int)status;

        var body = JsonSerializer.Serialize(new
        {
            title,
            status = (int)status,
            errors = errors?.ToArray()
        });

        return context.Response.WriteAsync(body);
    }
}
