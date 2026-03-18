using MediatR;
using Microsoft.Extensions.Logging;

namespace RecruitmentService.Application.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;

    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var name = typeof(TRequest).Name;
        _logger.LogInformation("[START] Handling {RequestName}: {@Request}", name, request);
        try
        {
            var response = await next(ct);
            _logger.LogInformation("[END] Handled {RequestName} successfully", name);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ERROR] Handling {RequestName} failed", name);
            throw;
        }
    }
}
