using MediatR;
using Microsoft.Extensions.Logging;

namespace TdsService.Application.Common.Behaviours;

/// <summary>
/// Logs all handled requests including performance warnings for slow requests.
/// </summary>
public sealed class LoggingBehaviour<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        => _logger = logger;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("TdsService Request: {Name} {@Request}", requestName, request);

        var response = await next();

        _logger.LogInformation("TdsService Response: {Name} {@Response}", requestName, response);

        return response;
    }
}
