using MediatR;
using Microsoft.Extensions.Logging;

namespace AdminService.Application.Behaviours;

/// <summary>
/// Logging pipeline behavior for MediatR
/// </summary>
public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling: {Name}", typeof(TRequest).Name);
        var response = await next();
        _logger.LogInformation("Completed: {Name}", typeof(TRequest).Name);
        return response;
    }
}
