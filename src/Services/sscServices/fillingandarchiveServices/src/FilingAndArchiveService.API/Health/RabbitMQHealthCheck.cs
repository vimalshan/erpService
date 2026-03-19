using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace FilingAndArchiveService.API.Health;

public class RabbitMQHealthCheck : IHealthCheck
{
    private readonly IConfiguration _configuration;

    public RabbitMQHealthCheck(IConfiguration configuration) => _configuration = configuration;

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var rabbitCfg = _configuration.GetSection("RabbitMQ");
            var factory = new global::RabbitMQ.Client.ConnectionFactory
            {
                HostName = rabbitCfg["Host"] ?? "localhost",
                UserName = rabbitCfg["Username"] ?? "guest",
                Password = rabbitCfg["Password"] ?? "guest",
                Port = int.TryParse(rabbitCfg["Port"], out var port) ? port : 5672,
                RequestedConnectionTimeout = TimeSpan.FromSeconds(3)
            };

            using var connection = factory.CreateConnectionAsync(cancellationToken).GetAwaiter().GetResult();
            return Task.FromResult(HealthCheckResult.Healthy("RabbitMQ connection is healthy."));
        }
        catch (Exception ex)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("RabbitMQ connection failed.", ex));
        }
    }
}
