using MediatR;
using Polly;
using Microsoft.Extensions.Logging;
using InsuranceManagement.Infrastructure.Resilience;

namespace InsuranceManagement.Application.CQRS.Behaviors;

/// <summary>
/// MediatR behavior to apply resilience policies to requests
/// </summary>
public class ResiliencePolicyBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IResiliencePolicyProvider _policyProvider;
    private readonly ILogger<ResiliencePolicyBehavior<TRequest, TResponse>> _logger;

    public ResiliencePolicyBehavior(
        IResiliencePolicyProvider policyProvider,
        ILogger<ResiliencePolicyBehavior<TRequest, TResponse>> logger)
    {
        _policyProvider = policyProvider ?? throw new ArgumentNullException(nameof(policyProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Executing request {typeof(TRequest).Name} with resilience policies");

        try
        {
            // Use combined policy (retry + circuit breaker) and timeout policy
            var combinedPolicy = _policyProvider.GetCombinedPolicy<TResponse>();
            var timeoutPolicy = _policyProvider.GetTimeoutPolicy<TResponse>();
            var wrappedPolicy = Policy.WrapAsync(combinedPolicy, timeoutPolicy);

            return await wrappedPolicy.ExecuteAsync(async () =>
            {
                return await next();
            });
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error executing request {typeof(TRequest).Name}: {ex.Message}");
            throw;
        }
    }
}
