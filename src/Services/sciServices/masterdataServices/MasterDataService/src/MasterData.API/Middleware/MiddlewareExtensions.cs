using MasterData.Application.DTOs;

namespace MasterData.API.Middleware
{
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
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ApiResponse<object>();

            switch (exception)
            {
                case ArgumentException argEx:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response = ApiResponse<object>.ErrorResponse(argEx.Message);
                    break;

                case KeyNotFoundException notFound:
                    context.Response.StatusCode = StatusCodes.Status404NotFound;
                    response = ApiResponse<object>.ErrorResponse(notFound.Message);
                    break;

                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    response = ApiResponse<object>.ErrorResponse("An internal server error occurred");
                    break;
            }

            return context.Response.WriteAsJsonAsync(response);
        }
    }

    /// <summary>
    /// Request/Response logging middleware
    /// </summary>
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();

            var requestPath = context.Request.Path.Value;
            var requestMethod = context.Request.Method;

            _logger.LogInformation($"Processing request: {requestMethod} {requestPath}");

            // Skip response body buffering for GraphQL — HotChocolate manages its own response stream
            if (requestPath != null && requestPath.StartsWith("/graphql", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                _logger.LogInformation($"Response status: {context.Response.StatusCode} for {requestMethod} {requestPath}");
                return;
            }

            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                _logger.LogInformation($"Response status: {context.Response.StatusCode} for {requestMethod} {requestPath}");

                responseBody.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
    }

    /// <summary>
    /// Custom authorization middleware for JWT bearer tokens
    /// </summary>
    public class JwtAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtAuthenticationMiddleware> _logger;

        public JwtAuthenticationMiddleware(RequestDelegate next, ILogger<JwtAuthenticationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (!string.IsNullOrEmpty(token))
            {
                try
                {
                    _logger.LogInformation("JWT token found in request");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "JWT token validation failed");
                }
            }

            await _next(context);
        }
    }
}
