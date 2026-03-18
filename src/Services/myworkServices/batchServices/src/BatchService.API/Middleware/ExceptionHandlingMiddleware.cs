using System.Net;
using System.Text.Json;
using FluentValidation;

namespace BatchService.API.Middleware;

/// <summary>Catches known exception types and converts them to structured JSON Problem Details responses.</summary>
public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (ValidationException vex)
        {
            _logger.LogWarning(vex, "Validation failed");
            await WriteErrorAsync(ctx, HttpStatusCode.BadRequest, "Validation Error",
                vex.Errors.Select(e => e.ErrorMessage));
        }
        catch (System.Collections.Generic.KeyNotFoundException kex)
        {
            _logger.LogWarning(kex, "Resource not found");
            await WriteErrorAsync(ctx, HttpStatusCode.NotFound, "Not Found", [kex.Message]);
        }
        catch (InvalidOperationException iex)
        {
            _logger.LogWarning(iex, "Invalid operation");
            await WriteErrorAsync(ctx, HttpStatusCode.Conflict, "Conflict", [iex.Message]);
        }
        catch (UnauthorizedAccessException uex)
        {
            _logger.LogWarning(uex, "Unauthorized");
            await WriteErrorAsync(ctx, HttpStatusCode.Unauthorized, "Unauthorized", [uex.Message]);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteErrorAsync(ctx, HttpStatusCode.InternalServerError, "Internal Server Error",
                ["An unexpected error occurred."]);
        }
    }

    private static async Task WriteErrorAsync(HttpContext ctx, HttpStatusCode status, string title, IEnumerable<string> errors)
    {
        ctx.Response.StatusCode  = (int)status;
        ctx.Response.ContentType = "application/problem+json";

        var problem = new
        {
            type    = $"https://httpstatuses.io/{(int)status}",
            title,
            status  = (int)status,
            errors
        };

        await ctx.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
