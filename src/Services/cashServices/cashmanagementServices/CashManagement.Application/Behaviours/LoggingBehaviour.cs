using MediatR;
using Microsoft.Extensions.Logging;

namespace CashManagement.Application.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
        => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("[CashManagement] {RequestName} started", requestName);
        try
        {
            var response = await next(cancellationToken);
            _logger.LogInformation("[CashManagement] {RequestName} completed successfully", requestName);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[CashManagement] {RequestName} failed", requestName);
            throw;
        }
    }
}
