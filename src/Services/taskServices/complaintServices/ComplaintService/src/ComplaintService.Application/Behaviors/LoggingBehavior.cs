using MediatR;
using Microsoft.Extensions.Logging;

namespace ComplaintService.Application.Behaviors;

public sealed class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var requestName = typeof(TRequest).Name;
        logger.LogInformation("[{Request}] Handling started", requestName);
        try
        {
            var response = await next();
            logger.LogInformation("[{Request}] Handling completed", requestName);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[{Request}] Handling failed: {Error}", requestName, ex.Message);
            throw;
        }
    }
}
