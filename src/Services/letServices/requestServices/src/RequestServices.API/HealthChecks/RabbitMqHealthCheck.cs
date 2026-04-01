using System.Net.Sockets;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace RequestServices.API.HealthChecks;

public class RabbitMqHealthCheck(IConfiguration configuration) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken ct = default)
    {
        var host = configuration["RabbitMQ:Host"] ?? "localhost";
        var port = int.Parse(configuration["RabbitMQ:Port"] ?? "5672");

        try
        {
            using var tcp = new TcpClient();
            await tcp.ConnectAsync(host, port, ct);
            return HealthCheckResult.Healthy("RabbitMQ is reachable.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("RabbitMQ is unreachable.", ex);
        }
    }
}
