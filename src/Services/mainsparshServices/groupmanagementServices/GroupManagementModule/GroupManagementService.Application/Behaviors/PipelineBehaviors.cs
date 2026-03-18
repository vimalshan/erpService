using MediatR;
using Microsoft.Extensions.Logging;

namespace GroupManagementService.Application.Behaviors
{
    /// <summary>
    /// Logging behavior for MediatR pipeline
    /// </summary>
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling {RequestName}", typeof(TRequest).Name);

            try
            {
                var response = await next();
                _logger.LogInformation("Successfully handled {RequestName}", typeof(TRequest).Name);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling {RequestName}: {Message}", typeof(TRequest).Name, ex.Message);
                throw;
            }
        }
    }

    /// <summary>
    /// Validation behavior for MediatR pipeline
    /// </summary>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;

        public ValidationBehavior(ILogger<ValidationBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // Validation logic can be added here
            // This is a placeholder for custom validation

            return await next();
        }
    }

    /// <summary>
    /// Exception handling behavior for MediatR pipeline
    /// </summary>
    public class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> _logger;

        public ExceptionHandlingBehavior(ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            try
            {
                return await next();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Business logic error: {Message}", ex.Message);
                throw;
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Argument error: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in {RequestName}: {Message}", typeof(TRequest).Name, ex.Message);
                throw;
            }
        }
    }

    /// <summary>
    /// Performance monitoring behavior for MediatR pipeline
    /// </summary>
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;

        public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var startTime = DateTime.UtcNow;

            try
            {
                return await next();
            }
            finally
            {
                var elapsedTime = DateTime.UtcNow - startTime;
                if (elapsedTime.TotalMilliseconds > 500)
                {
                    _logger.LogWarning("Long-running request {RequestName} took {ElapsedMilliseconds}ms", 
                        typeof(TRequest).Name, elapsedTime.TotalMilliseconds);
                }
            }
        }
    }
}
