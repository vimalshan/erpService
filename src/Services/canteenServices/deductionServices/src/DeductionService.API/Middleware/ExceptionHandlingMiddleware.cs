using System.Net;
using System.Text.Json;
using DeductionService.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace DeductionService.API.Middleware;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            logger.LogWarning(ex, "Validation error on {Path}", context.Request.Path);
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/problem+json";

            var problem = new ValidationProblemDetails
            {
                Title = "Validation Failed",
                Status = (int)HttpStatusCode.BadRequest,
                Detail = "One or more validation errors occurred."
            };
            foreach (var err in ex.Errors)
                problem.Errors.TryAdd(err.PropertyName, [err.ErrorMessage]);

            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
        catch (DeductionDomainException ex)
        {
            logger.LogWarning(ex, "Domain error on {Path}", context.Request.Path);
            context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
            context.Response.ContentType = "application/problem+json";

            var problem = new ProblemDetails
            {
                Title = "Domain Rule Violation",
                Status = (int)HttpStatusCode.UnprocessableEntity,
                Detail = ex.Message
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception on {Path}", context.Request.Path);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/problem+json";

            var problem = new ProblemDetails
            {
                Title = "Internal Server Error",
                Status = (int)HttpStatusCode.InternalServerError,
                Detail = "An unexpected error occurred. Please try again later."
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }
}
