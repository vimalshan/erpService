using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.Http;

namespace ApiGateway.HealthChecks;

public static class HealthCheckExtensions
{
    private static readonly Dictionary<string, (string ConfigKey, string DefaultHost, int DefaultPort)> ServiceDefaults = new()
    {
        ["access-service"] = ("Services:AccessService", "localhost", 5010),
        ["attendance-service"] = ("Services:AttendanceService", "localhost", 5011),
        ["bus-service"] = ("Services:BusService", "localhost", 5012),
        ["calendar-service"] = ("Services:CalendarService", "localhost", 5013),
        ["employee-service"] = ("Services:EmployeeService", "localhost", 5014),
        ["groupincentive-service"] = ("Services:GroupIncentiveService", "localhost", 5015),
        ["leave-service"] = ("Services:LeaveService", "localhost", 5016),
        ["reference-service"] = ("Services:ReferenceService", "localhost", 5017),
        ["visitor-service"] = ("Services:VisitorService", "localhost", 5018)
    };

    public static IServiceCollection AddGatewayHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var hcBuilder = services.AddHealthChecks();

        // Add a self-check that always returns Healthy (gateway is running)
        hcBuilder.Add(new HealthCheckRegistration(
            "self",
            sp => new SelfHealthCheck(),
            HealthStatus.Healthy,
            new[] { "self" }));

        foreach (var (name, defaults) in ServiceDefaults)
        {
            var baseUrl = configuration[defaults.ConfigKey] ?? $"http://{defaults.DefaultHost}:{defaults.DefaultPort}";
            hcBuilder.Add(new HealthCheckRegistration(
                name,
                sp => new DownstreamServiceHealthCheck(name, baseUrl),
                HealthStatus.Degraded,
                new[] { "downstream", name }));
        }

        // RabbitMQ connectivity check
        var rabbitHost = configuration["RabbitMQ:Host"] ?? "localhost";
        var rabbitPort = int.Parse(configuration["RabbitMQ:Port"] ?? "5672");
        hcBuilder.Add(new HealthCheckRegistration(
            "rabbitmq",
            sp => new RabbitMqHealthCheck(rabbitHost, rabbitPort),
            HealthStatus.Degraded,
            new[] { "infrastructure", "rabbitmq" }));

        return services;
    }
}

public class DownstreamServiceHealthCheck : IHealthCheck
{
    private readonly string _serviceName;
    private readonly string _baseUrl;
    private static readonly HttpClient HttpClient = new() { Timeout = TimeSpan.FromSeconds(5) };

    public DownstreamServiceHealthCheck(string serviceName, string baseUrl)
    {
        _serviceName = serviceName;
        _baseUrl = baseUrl.TrimEnd('/');
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await HttpClient.GetAsync($"{_baseUrl}/health", cancellationToken);
            return response.IsSuccessStatusCode
                ? HealthCheckResult.Healthy($"{_serviceName} is healthy")
                : HealthCheckResult.Degraded($"{_serviceName} returned {response.StatusCode}");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Degraded($"{_serviceName} is unreachable: {ex.Message}");
        }
    }
}

public class SelfHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy("API Gateway is running"));
    }
}

public class RabbitMqHealthCheck : IHealthCheck
{
    private readonly string _host;
    private readonly int _port;

    public RabbitMqHealthCheck(string host, int port)
    {
        _host = host;
        _port = port;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using var tcp = new System.Net.Sockets.TcpClient();
            await tcp.ConnectAsync(_host, _port, cancellationToken);
            return HealthCheckResult.Healthy("RabbitMQ is reachable");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Degraded($"RabbitMQ is unreachable: {ex.Message}");
        }
    }
}
