namespace CommunityService.API.Middleware;

using System.Net;
using System.Text.Json;
using FluentValidation;

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
            _logger.LogError(ex, "Exception caught");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        object problemDetails;
        int statusCode;

        switch (exception)
        {
            case ValidationException validationEx:
                statusCode = (int)HttpStatusCode.BadRequest;
                problemDetails = new
                {
                    status = statusCode,
                    message = "Validation failed",
                    type = "ValidationException",
                    errors = validationEx.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage })
                };
                break;

            case ArgumentException:
                statusCode = (int)HttpStatusCode.BadRequest;
                problemDetails = new { status = statusCode, message = exception.Message, type = exception.GetType().Name };
                break;

            case InvalidOperationException:
                statusCode = (int)HttpStatusCode.Conflict;
                problemDetails = new { status = statusCode, message = exception.Message, type = exception.GetType().Name };
                break;

            default:
                statusCode = (int)HttpStatusCode.InternalServerError;
                problemDetails = new { status = statusCode, message = exception.Message, type = exception.GetType().Name };
                break;
        }

        response.StatusCode = statusCode;
        var json = System.Text.Json.JsonSerializer.Serialize(problemDetails);
        return response.WriteAsync(json);
    }
}

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthenticationMiddleware> _logger;

    public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        
        if (!string.IsNullOrEmpty(token))
        {
            context.Items["Token"] = token;
            _logger.LogInformation("Authorization token found");
        }

        await _next(context);
    }
}
