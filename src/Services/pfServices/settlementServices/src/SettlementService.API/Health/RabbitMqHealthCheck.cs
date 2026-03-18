using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;

namespace SettlementService.API.Health;

public class RabbitMqHealthCheck : IHealthCheck
{
    private readonly string _hostName;
    private readonly int _port;
    private readonly string _userName;
    private readonly string _password;

    public RabbitMqHealthCheck(string hostName, int port, string userName, string password)
    {
        _hostName = hostName;
        _port = port;
        _userName = userName;
        _password = password;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var factory = new ConnectionFactory
            {
                HostName = _hostName,
                Port = _port,
                UserName = _userName,
                Password = _password,
                RequestedConnectionTimeout = TimeSpan.FromSeconds(3)
            };

            await using var connection = await factory.CreateConnectionAsync(cancellationToken);
            return HealthCheckResult.Healthy("RabbitMQ is reachable.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Degraded($"RabbitMQ is not reachable: {ex.Message}");
        }
    }
}
