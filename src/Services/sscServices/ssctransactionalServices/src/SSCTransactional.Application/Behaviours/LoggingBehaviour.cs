using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace SSCTransactional.Application.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("[Request] Handling {RequestName} {@Request}", requestName, request);
        var response = await next(cancellationToken);
        _logger.LogInformation("[Response] {RequestName} handled successfully", requestName);
        return response;
    }
}
