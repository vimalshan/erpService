using AuditService.Infrastructure.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace AuditService.Infrastructure.HealthChecks;

public sealed class DatabaseHealthCheck : IHealthCheck
{
    private readonly AuditDbContext _context;

    public DatabaseHealthCheck(AuditDbContext context) => _context = context;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var canConnect = await _context.Database.CanConnectAsync(cancellationToken);
            return canConnect
                ? HealthCheckResult.Healthy("Database connection is healthy.")
                : HealthCheckResult.Unhealthy("Cannot connect to database.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("Database health check failed.", ex);
        }
    }
}

public sealed class RabbitMQHealthCheck : IHealthCheck
{
    private readonly string _hostName;
    private readonly int _port;

    public RabbitMQHealthCheck(string hostName, int port)
    {
        _hostName = hostName;
        _port = port;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using var client = new System.Net.Sockets.TcpClient();
            var connectTask = client.ConnectAsync(_hostName, _port);
            var timeoutTask = Task.Delay(2000, cancellationToken);
            var completed = Task.WhenAny(connectTask, timeoutTask).Result;

            if (completed == connectTask && client.Connected)
                return Task.FromResult(HealthCheckResult.Healthy("RabbitMQ is reachable."));

            return Task.FromResult(HealthCheckResult.Degraded("RabbitMQ is not reachable within timeout."));
        }
        catch (Exception ex)
        {
            return Task.FromResult(HealthCheckResult.Unhealthy("RabbitMQ health check failed.", ex));
        }
    }
}
