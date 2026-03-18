using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace LocationServices.API.Middleware;

/// <summary>
/// Global exception handler — transforms unhandled exceptions into RFC 7807
/// Problem Details responses and ensures no stack traces leak to clients.
/// </summary>
public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next   = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext ctx)
    {
        try
        {
            await _next(ctx);
        }
        catch (FluentValidation.ValidationException vex)
        {
            _logger.LogWarning("[Validation] {Errors}", string.Join("; ", vex.Errors.Select(e => e.ErrorMessage)));
            await WriteProblemAsync(ctx, StatusCodes.Status400BadRequest,
                "Validation Failed", vex.Message,
                new Dictionary<string, object?> { ["errors"] = vex.Errors.Select(e => e.ErrorMessage) });
        }
        catch (UnauthorizedAccessException)
        {
            await WriteProblemAsync(ctx, StatusCodes.Status401Unauthorized, "Unauthorized", "Authentication required.");
        }
        catch (System.Collections.Generic.KeyNotFoundException knfex)
        {
            await WriteProblemAsync(ctx, StatusCodes.Status404NotFound, "Not Found", knfex.Message);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("[Request] Cancelled by client.");
            ctx.Response.StatusCode = StatusCodes.Status499ClientClosedRequest;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[Unhandled] {Message}", ex.Message);
            await WriteProblemAsync(ctx, StatusCodes.Status500InternalServerError,
                "Internal Server Error",
                "An unexpected error occurred. Please try again later.");
        }
    }

    private static async Task WriteProblemAsync(HttpContext ctx, int status,
        string title, string detail, IDictionary<string, object?>? extensions = null)
    {
        ctx.Response.StatusCode  = status;
        ctx.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Title   = title,
            Detail  = detail,
            Status  = status,
            Instance = ctx.Request.Path
        };

        if (extensions is not null)
            foreach (var (k, v) in extensions)
                problem.Extensions[k] = v;

        await ctx.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
