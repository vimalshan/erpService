using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace EmployeeManagement.Application.Common.Behaviours;

public sealed class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("EmployeeManagement Request: {RequestName} {@Request}", requestName, request);

        TResponse response;
        var sw = Stopwatch.StartNew();
        try
        {
            response = await next();
        }
        finally
        {
            sw.Stop();
            _logger.LogInformation("EmployeeManagement Response: {RequestName} completed in {ElapsedMs}ms", requestName, sw.ElapsedMilliseconds);
        }

        return response;
    }
}
