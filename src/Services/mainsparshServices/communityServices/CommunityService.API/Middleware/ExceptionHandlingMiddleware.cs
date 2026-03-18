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

        dynamic problemDetails;
        problemDetails = new
        {
            status = response.StatusCode,
            message = exception.Message,
            type = exception.GetType().Name
        };

        switch (exception)
        {
            case ValidationException validationEx:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                problemDetails = new
                {
                    status = response.StatusCode,
                    message = "Validation failed",
                    type = "ValidationException",
                    errors = validationEx.Errors.Select(e => new { field = e.PropertyName, message = e.ErrorMessage })
                };
                break;

            case ArgumentException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                problemDetails.status = (int)HttpStatusCode.BadRequest;
                break;

            case InvalidOperationException:
                response.StatusCode = (int)HttpStatusCode.Conflict;
                problemDetails.status = (int)HttpStatusCode.Conflict;
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                problemDetails.status = (int)HttpStatusCode.InternalServerError;
                break;
        }

        var json = System.Text.Json.JsonSerializer.Serialize((object)problemDetails);
        response.ContentType = "application/json";
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
