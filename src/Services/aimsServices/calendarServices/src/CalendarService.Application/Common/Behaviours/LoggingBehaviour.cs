using MediatR;
using Microsoft.Extensions.Logging;

namespace CalendarService.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest, TResponse>(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var typeName = typeof(TRequest).Name;
        logger.LogInformation("Handling {RequestType}: {@Request}", typeName, request);
        var response = await next();
        logger.LogInformation("Handled {RequestType} successfully", typeName);
        return response;
    }
}
