using MediatR;
using Microsoft.Extensions.Logging;

namespace ReceivingService.Application.Behaviours;

/// <summary>Logs each MediatR request and its result.</summary>
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
        _logger.LogInformation("Handling {Request}", requestName);
        var response = await next(cancellationToken);
        _logger.LogInformation("Handled {Request}", requestName);
        return response;
    }
}
