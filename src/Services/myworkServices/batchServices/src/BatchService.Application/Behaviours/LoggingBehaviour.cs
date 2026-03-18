using MediatR;
using Microsoft.Extensions.Logging;

namespace BatchService.Application.Behaviours;

/// <summary>Logs request entry, exit and elapsed time.</summary>
public sealed class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("[CQRS] Handling {RequestName}", requestName);
        var response = await next(ct);
        _logger.LogInformation("[CQRS] Handled  {RequestName}", requestName);
        return response;
    }
}
