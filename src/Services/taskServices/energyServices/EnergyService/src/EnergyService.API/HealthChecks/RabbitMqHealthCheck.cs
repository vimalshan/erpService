using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;

namespace EnergyService.API.HealthChecks;

public class RabbitMqHealthCheck : IHealthCheck
{
    private readonly IConnectionFactory _connectionFactory;

    public RabbitMqHealthCheck(IConnectionFactory connectionFactory) => _connectionFactory = connectionFactory;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = default)
    {
        try
        {
            using var connection = await _connectionFactory.CreateConnectionAsync(ct);
            return HealthCheckResult.Healthy("RabbitMQ connection is healthy.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("RabbitMQ connection failed.", ex);
        }
    }
}
