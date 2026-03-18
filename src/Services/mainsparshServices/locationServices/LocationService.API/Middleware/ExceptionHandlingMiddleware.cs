using System.Net;
using LocationService.Domain.Exceptions;

namespace LocationService.API.Middleware
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new { message = "An internal server error occurred.", details = "", statusCode = 0 };

            switch (exception)
            {
                case EntityNotFoundException notFound:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response = new { message = notFound.Message, details = "", statusCode = (int)HttpStatusCode.NotFound };
                    break;

                case EntityAlreadyExistsException alreadyExists:
                    context.Response.StatusCode = (int)HttpStatusCode.Conflict;
                    response = new { message = alreadyExists.Message, details = "", statusCode = (int)HttpStatusCode.Conflict };
                    break;

                case BusinessRuleException businessRule:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response = new { message = businessRule.Message, details = businessRule.Code, statusCode = (int)HttpStatusCode.BadRequest };
                    break;

                case DomainException domain:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response = new { message = domain.Message, details = "", statusCode = (int)HttpStatusCode.BadRequest };
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response = new { message = "An unexpected error occurred.", details = exception.Message, statusCode = (int)HttpStatusCode.InternalServerError };
                    break;
            }

            return context.Response.WriteAsJsonAsync(response);
        }
    }

    /// <summary>
    /// Extension method to add exception handling middleware
    /// </summary>
    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
