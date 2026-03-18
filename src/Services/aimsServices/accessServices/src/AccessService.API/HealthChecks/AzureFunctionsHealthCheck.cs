using Microsoft.Extensions.Diagnostics.HealthChecks;
using AccessService.Infrastructure.AzureFunctions;

namespace AccessService.API.HealthChecks
{
    /// <summary>
    /// Health check for Azure Functions connectivity
    /// </summary>
    public class AzureFunctionsHealthCheck : IHealthCheck
    {
        private readonly IAzureFunctionsService _functionsService;

        public AzureFunctionsHealthCheck(IAzureFunctionsService functionsService)
        {
            _functionsService = functionsService ?? throw new ArgumentNullException(nameof(functionsService));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var isConnected = await _functionsService.IsConnectedAsync();

                if (isConnected)
                {
                    return HealthCheckResult.Healthy("Azure Functions service is healthy");
                }
                else
                {
                    return HealthCheckResult.Degraded("Azure Functions service is not available");
                }
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Azure Functions health check failed", ex);
            }
        }
    }
}
