using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace CompetencyService.Application.Behaviours;

/// <summary>Logs every request with elapsed time. Warns if > 500ms.</summary>
public class LoggingBehaviour<TRequest, TResponse>(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var requestName = typeof(TRequest).Name;
        logger.LogInformation("Handling {RequestName}", requestName);
        var sw = Stopwatch.StartNew();
        var response = await next();
        sw.Stop();
        if (sw.ElapsedMilliseconds > 500)
            logger.LogWarning("Slow request {RequestName} took {Elapsed}ms", requestName, sw.ElapsedMilliseconds);
        else
            logger.LogInformation("Handled {RequestName} in {Elapsed}ms", requestName, sw.ElapsedMilliseconds);
        return response;
    }
}
