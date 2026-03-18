namespace FinyearAPI.Gateway.Middleware
{
    /// <summary>
    /// Exception handling middleware for global error handling
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
                _logger.LogError(ex, "Unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = new ErrorResponse();

            switch (exception)
            {
                case ArgumentException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response.Message = exception.Message;
                    break;
                case KeyNotFoundException:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    response.Message = "Resource not found";
                    break;
                case UnauthorizedAccessException:
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    response.Message = "Unauthorized access";
                    break;
                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    response.Message = "Internal server error";
                    break;
            }

            response.Status = context.Response.StatusCode;
            response.TraceId = context.TraceIdentifier;
            return context.Response.WriteAsJsonAsync(response);
        }
    }

    /// <summary>
    /// Error response model
    /// </summary>
    public class ErrorResponse
    {
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;
        public string TraceId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Request/Response logging middleware
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var startTime = DateTime.UtcNow;
            var originalBodyStream = context.Response.Body;

            try
            {
                _logger.LogInformation(
                    "Incoming Request: {Method} {Path} {QueryString}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Request.QueryString);

                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;

                    await _next(context);

                    var duration = DateTime.UtcNow - startTime;
                    _logger.LogInformation(
                        "Response: {StatusCode} {Method} {Path} - Duration: {Duration}ms",
                        context.Response.StatusCode,
                        context.Request.Method,
                        context.Request.Path,
                        duration.TotalMilliseconds);

                    await responseBody.CopyToAsync(originalBodyStream);
                }
            }
            finally
            {
                context.Response.Body = originalBodyStream;
            }
        }
    }

    /// <summary>
    /// API versioning middleware
    /// Handles API v1, v2, etc.
    /// </summary>
    public class ApiVersioningMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiVersioningMiddleware> _logger;

        public ApiVersioningMiddleware(RequestDelegate next, ILogger<ApiVersioningMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Extract version from header or route
            var version = context.Request.Headers["api-version"].FirstOrDefault() ?? "1.0";
            context.Items["ApIVersion"] = version;

            _logger.LogInformation("API Version: {Version}", version);

            await _next(context);
        }
    }

    /// <summary>
    /// Authentication middleware
    /// </summary>
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
                _logger.LogInformation("JWT Token received for authentication");
                // Actual JWT validation would happen here
                // context.User would be set with claims from token
            }

            await _next(context);
        }
    }

    /// <summary>
    /// CORS configuration middleware
    /// </summary>
    public static class CorsConfiguration
    {
        public const string AllowSpecificOrigins = "AllowSpecificOrigins";

        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(AllowSpecificOrigins, builder =>
                {
                    builder.WithOrigins(
                            "http://localhost:3000",
                            "http://localhost:4200",
                            "https://localhost:7136",
                            "https://localhost:7001")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithExposedHeaders("X-Total-Count", "X-Page-Number");
                });
            });

            return services;
        }
    }
}
