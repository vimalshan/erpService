using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LetTransactionService.Application.Common.Behaviours;

public sealed class LoggingBehaviour<TRequest, TResponse>(
    ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var requestName = typeof(TRequest).Name;
        logger.LogInformation("LetTransactionService - Handling {RequestName}: {@Request}", requestName, request);

        var sw = Stopwatch.StartNew();
        var response = await next();
        sw.Stop();

        logger.LogInformation("LetTransactionService - Handled {RequestName} in {ElapsedMilliseconds}ms",
            requestName, sw.ElapsedMilliseconds);
        return response;
    }
}
