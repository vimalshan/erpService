using MediatR;
using Microsoft.Extensions.Logging;

namespace AttendanceService.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest, TResponse>(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var name = typeof(TRequest).Name;
        logger.LogInformation("Handling {RequestName}: {@Request}", name, request);
        var response = await next();
        logger.LogInformation("Handled {RequestName}", name);
        return response;
    }
}
