using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace ApiGateway.Functions.Functions;

public sealed class HealthMonitorFunction
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<HealthMonitorFunction> _logger;

    private static readonly Dictionary<string, string> ServiceEndpoints = new()
    {
        ["CashManagement"] = "http://localhost:5249",
        ["CurrencyManagement"] = "http://localhost:5031",
        ["DealTicketing"] = "http://localhost:5081",
        ["LoanManagement"] = "http://localhost:5268",
        ["OrganizationSetup"] = "http://localhost:5099",
        ["EmailNotification"] = "http://localhost:5032"
    };

    public HealthMonitorFunction(IHttpClientFactory httpClientFactory, ILogger<HealthMonitorFunction> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    [Function("HealthMonitor")]
    public async Task<HealthMonitorResult> RunScheduled(
        [TimerTrigger("0 */5 * * * *")] TimerInfo timer)
    {
        _logger.LogInformation("Health monitor triggered at {Time}", DateTime.UtcNow);
        return await CheckAllServicesAsync();
    }

    [Function("HealthMonitorHttp")]
    public async Task<HttpResponseData> RunHttp(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "health-monitor")] HttpRequestData req)
    {
        var result = await CheckAllServicesAsync();

        var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
        await response.WriteAsJsonAsync(result);
        return response;
    }

    private async Task<HealthMonitorResult> CheckAllServicesAsync()
    {
        using var client = _httpClientFactory.CreateClient();
        client.Timeout = TimeSpan.FromSeconds(5);

        var checks = new List<ServiceHealthResult>();

        foreach (var (name, baseUrl) in ServiceEndpoints)
        {
            try
            {
                var response = await client.GetAsync($"{baseUrl}/health");
                checks.Add(new ServiceHealthResult
                {
                    ServiceName = name,
                    BaseUrl = baseUrl,
                    Status = response.IsSuccessStatusCode ? "Healthy" : "Unhealthy",
                    StatusCode = (int)response.StatusCode,
                    CheckedAt = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Health check failed for {Service}", name);
                checks.Add(new ServiceHealthResult
                {
                    ServiceName = name,
                    BaseUrl = baseUrl,
                    Status = "Unavailable",
                    StatusCode = 0,
                    Error = ex.Message,
                    CheckedAt = DateTime.UtcNow
                });
            }
        }

        var allHealthy = checks.All(c => c.Status == "Healthy");
        var anyHealthy = checks.Any(c => c.Status == "Healthy");

        return new HealthMonitorResult
        {
            OverallStatus = allHealthy ? "Healthy" : anyHealthy ? "Degraded" : "Unhealthy",
            CheckedAt = DateTime.UtcNow,
            Services = checks
        };
    }
}

public sealed class HealthMonitorResult
{
    public string OverallStatus { get; set; } = string.Empty;
    public DateTime CheckedAt { get; set; }
    public List<ServiceHealthResult> Services { get; set; } = [];
}

public sealed class ServiceHealthResult
{
    public string ServiceName { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public string? Error { get; set; }
    public DateTime CheckedAt { get; set; }
}
