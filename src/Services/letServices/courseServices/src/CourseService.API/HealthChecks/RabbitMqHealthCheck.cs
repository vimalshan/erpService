using System.Net.Sockets;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CourseService.API.HealthChecks;

public class RabbitMqHealthCheck(IConfiguration configuration) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = default)
    {
        var host = configuration["RabbitMQ:Host"] ?? "localhost";
        var port = configuration.GetValue<int?>("RabbitMQ:Port") ?? 5672;

        using var client = new TcpClient();

        try
        {
            using var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            timeoutCts.CancelAfter(TimeSpan.FromSeconds(2));

            await client.ConnectAsync(host, port, timeoutCts.Token);
            return HealthCheckResult.Healthy($"RabbitMQ is reachable at {host}:{port}.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Degraded($"RabbitMQ is not reachable at {host}:{port}.", ex);
        }
    }
}
