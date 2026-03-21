using MediatR;
using Microsoft.Extensions.Logging;

namespace RackingSystem.Application.Common.Behaviours;

public sealed class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger) =>
        _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("RackingSystem Request: {RequestName} {@Request}", requestName, request);

        try
        {
            var response = await next(ct);
            _logger.LogInformation("RackingSystem Response: {RequestName} {@Response}", requestName, response);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RackingSystem Request Error: {RequestName} {@Request}", requestName, request);
            throw;
        }
    }
}
