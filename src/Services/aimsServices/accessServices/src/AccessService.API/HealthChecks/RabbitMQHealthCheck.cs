using Microsoft.Extensions.Diagnostics.HealthChecks;
using AccessService.Infrastructure.MessageBrokers.RabbitMQ;

namespace AccessService.API.HealthChecks
{
    /// <summary>
    /// Health check for RabbitMQ message broker connection
    /// </summary>
    public class RabbitMQHealthCheck : IHealthCheck
    {
        private readonly IRabbitMQConnection _rabbitMqConnection;

        public RabbitMQHealthCheck(IRabbitMQConnection rabbitMqConnection)
        {
            _rabbitMqConnection = rabbitMqConnection ?? throw new ArgumentNullException(nameof(rabbitMqConnection));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var isConnected = await _rabbitMqConnection.IsConnectedAsync();

                if (isConnected)
                {
                    return HealthCheckResult.Healthy("RabbitMQ connection is healthy");
                }
                else
                {
                    return HealthCheckResult.Degraded("RabbitMQ connection is not available");
                }
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("RabbitMQ health check failed", ex);
            }
        }
    }
}
