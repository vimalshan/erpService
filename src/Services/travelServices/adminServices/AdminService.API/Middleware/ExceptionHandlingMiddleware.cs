namespace AdminService.API.Middleware;

/// <summary>
/// Global exception handling middleware
/// </summary>
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
            _logger.LogError(ex, "An unexpected error occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new { message = exception.Message, statusCode = context.Response.StatusCode };

        return exception switch
        {
            InvalidOperationException => 
                Handle(context, 404, "Resource not found"),
            ArgumentException => 
                Handle(context, 400, exception.Message),
            FluentValidation.ValidationException validationEx =>
                Handle(context, 400, "Validation failed", validationEx.Errors.Select(e => e.ErrorMessage)),
            _ => Handle(context, 500, "Internal server error")
        };

        static Task Handle(HttpContext context, int statusCode, string message, IEnumerable<string>? errors = null)
        {
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsJsonAsync(new { message, errors });
        }
    }
}
