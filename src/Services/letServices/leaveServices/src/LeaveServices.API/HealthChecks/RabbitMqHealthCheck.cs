using System.Net.Sockets;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LeaveServices.API.HealthChecks;

public sealed class RabbitMqHealthCheck : IHealthCheck
{
    private readonly IConfiguration _configuration;

    public RabbitMqHealthCheck(IConfiguration configuration) => _configuration = configuration;

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var host = _configuration["RabbitMQ:Host"] ?? "localhost";
        var port = int.TryParse(_configuration["RabbitMQ:Port"], out var p) ? p : 5672;

        try
        {
            using var client = new TcpClient();
            await client.ConnectAsync(host, port, cancellationToken);
            return HealthCheckResult.Healthy($"RabbitMQ reachable at {host}:{port}");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Degraded($"RabbitMQ unreachable at {host}:{port}", ex);
        }
    }
}
