using System.Net.Sockets;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ReviewService.API.HealthChecks;

public class RabbitMqHealthCheck : IHealthCheck
{
    private readonly string _host;
    private readonly int _port;

    public RabbitMqHealthCheck(IConfiguration configuration)
    {
        _host = configuration["RabbitMQ:Host"] ?? "localhost";
        _port = int.TryParse(configuration["RabbitMQ:Port"], out var p) ? p : 5672;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using var tcp = new TcpClient();
            await tcp.ConnectAsync(_host, _port, cancellationToken);
            return HealthCheckResult.Healthy("RabbitMQ reachable");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("RabbitMQ unreachable", ex);
        }
    }
}
