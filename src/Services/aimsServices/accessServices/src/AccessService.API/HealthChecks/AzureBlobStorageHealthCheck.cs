using Microsoft.Extensions.Diagnostics.HealthChecks;
using AccessService.Infrastructure.BlobStorage;

namespace AccessService.API.HealthChecks
{
    /// <summary>
    /// Health check for Azure Blob Storage connectivity
    /// </summary>
    public class AzureBlobStorageHealthCheck : IHealthCheck
    {
        private readonly IAzureBlobStorageService _blobStorageService;

        public AzureBlobStorageHealthCheck(IAzureBlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService ?? throw new ArgumentNullException(nameof(blobStorageService));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var isConnected = await _blobStorageService.IsConnectedAsync();

                if (isConnected)
                {
                    return HealthCheckResult.Healthy("Azure Blob Storage connection is healthy");
                }
                else
                {
                    return HealthCheckResult.Degraded("Azure Blob Storage connection is not available");
                }
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Azure Blob Storage health check failed", ex);
            }
        }
    }
}
