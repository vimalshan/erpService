using MediatR;

namespace ReportingService.Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        System.Diagnostics.Debug.WriteLine($"[CQRS] Handling {requestName}");

        try
        {
            var response = await next();
            System.Diagnostics.Debug.WriteLine($"[CQRS] Completed {requestName}");
            return response;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[CQRS] Error handling {requestName}: {ex.Message}");
            throw;
        }
    }
}
