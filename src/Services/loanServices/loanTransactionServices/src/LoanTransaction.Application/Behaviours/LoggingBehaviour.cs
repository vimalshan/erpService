using MediatR;
using Microsoft.Extensions.Logging;

namespace LoanTransaction.Application.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        _logger.LogInformation("Handling {RequestName}: {@Request}", typeof(TRequest).Name, request);
        var response = await next(ct);
        _logger.LogInformation("Handled {RequestName} successfully", typeof(TRequest).Name);
        return response;
    }
}
