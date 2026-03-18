using MediatR;
using Microsoft.Extensions.Logging;

namespace LeaveServices.Application.Behaviours;

/// <summary>
/// Pipeline behaviour that logs every incoming request and its response.
/// </summary>
public sealed class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        _logger.LogInformation("Handling {RequestName} {@Request}", typeof(TRequest).Name, request);
        var response = await next();
        _logger.LogInformation("Handled  {RequestName}", typeof(TRequest).Name);
        return response;
    }
}
