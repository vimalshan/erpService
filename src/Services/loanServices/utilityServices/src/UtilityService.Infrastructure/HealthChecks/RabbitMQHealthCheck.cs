using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using UtilityService.Infrastructure.Messaging;

namespace UtilityService.Infrastructure.HealthChecks;

public class RabbitMQHealthCheck : IHealthCheck
{
    private readonly RabbitMQSettings _settings;

    public RabbitMQHealthCheck(IOptions<RabbitMQSettings> settings) => _settings = settings.Value;

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _settings.Host,
                Port = _settings.Port,
                VirtualHost = _settings.VirtualHost,
                UserName = _settings.Username,
                Password = _settings.Password,
                RequestedConnectionTimeout = TimeSpan.FromSeconds(3)
            };

            await using var connection = await factory.CreateConnectionAsync(cancellationToken);
            await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            return HealthCheckResult.Healthy("RabbitMQ connection is healthy.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Degraded(
                $"RabbitMQ is unavailable: {ex.Message}",
                exception: ex);
        }
    }
}
