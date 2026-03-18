using MediatR;
using Microsoft.Extensions.Logging;

namespace TrainingDevelopment.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("TrainingDevelopment Request: {Name} {@Request}", requestName, request);
        try
        {
            var response = await next();
            _logger.LogInformation("TrainingDevelopment Response: {Name} {@Response}", requestName, response);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "TrainingDevelopment Request: Unhandled exception for {Name}", requestName);
            throw;
        }
    }
}
