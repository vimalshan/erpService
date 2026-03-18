using GSTComplianceService.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GSTComplianceService.API.Middleware;

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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Unhandled exception for request {Method} {Path}",
            context.Request.Method, context.Request.Path);

        var (statusCode, title, detail, errors) = exception switch
        {
            ValidationException ve => (HttpStatusCode.BadRequest, "Validation Failed", ve.Message, ve.Errors),
            NotFoundException nfe => (HttpStatusCode.NotFound, "Not Found", nfe.Message, (IDictionary<string, string[]>?)null),
            Domain.Exceptions.DuplicatePanException dpe => (HttpStatusCode.Conflict, "Conflict", dpe.Message, (IDictionary<string, string[]>?)null),
            ForbiddenAccessException => (HttpStatusCode.Forbidden, "Forbidden", "Access denied.", (IDictionary<string, string[]>?)null),
            _ => (HttpStatusCode.InternalServerError, "Server Error", "An unexpected error occurred.", (IDictionary<string, string[]>?)null)
        };

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/problem+json";

        var problem = new ProblemDetails
        {
            Status = (int)statusCode,
            Title = title,
            Detail = detail,
            Instance = context.Request.Path
        };

        if (errors is not null)
            problem.Extensions["errors"] = errors;

        await context.Response.WriteAsJsonAsync(problem);
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        => app.UseMiddleware<ExceptionHandlingMiddleware>();
}
