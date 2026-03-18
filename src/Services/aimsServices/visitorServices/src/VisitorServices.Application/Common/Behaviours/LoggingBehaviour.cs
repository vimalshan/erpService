using MediatR;
using Microsoft.Extensions.Logging;

namespace VisitorServices.Application.Common.Behaviours;

public sealed class LoggingBehaviour<TRequest, TResponse>(
    ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {RequestName}: {@Request}", typeof(TRequest).Name, request);
        var response = await next(cancellationToken);
        logger.LogInformation("Handled {RequestName}", typeof(TRequest).Name);
        return response;
    }
}
