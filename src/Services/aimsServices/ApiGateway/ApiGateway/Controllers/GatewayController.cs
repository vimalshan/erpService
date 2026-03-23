using ApiGateway.Resilience;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GatewayController : ControllerBase
{
    private readonly CircuitBreakerStateService _circuitBreaker;
    private readonly ILogger<GatewayController> _logger;

    private static readonly Dictionary<string, ServiceConfig> Services = new()
    {
        ["access"] = new("access-service", 5010, "Access & Security Management"),
        ["attendance"] = new("attendance-service", 5011, "Attendance Tracking"),
        ["bus"] = new("bus-service", 5012, "Bus Transport Management"),
        ["calendar"] = new("calendar-service", 5013, "Calendar & Events"),
        ["employee"] = new("employee-service", 5014, "Employee Management"),
        ["groupincentive"] = new("groupincentive-service", 5015, "Group Incentive Management"),
        ["leave"] = new("leave-service", 5016, "Leave Management"),
        ["reference"] = new("reference-service", 5017, "Reference Data"),
        ["visitor"] = new("visitor-service", 5018, "Visitor Management")
    };

    public GatewayController(CircuitBreakerStateService circuitBreaker, ILogger<GatewayController> logger)
    {
        _circuitBreaker = circuitBreaker;
        _logger = logger;
    }

    /// <summary>
    /// Lists all registered downstream services.
    /// </summary>
    [HttpGet("services")]
    public IActionResult GetServices()
    {
        var result = Services.Select(s => new
        {
            Key = s.Key,
            Name = s.Value.Name,
            Port = s.Value.Port,
            Description = s.Value.Description,
            Url = $"http://localhost:{s.Value.Port}"
        });
        return Ok(result);
    }

    /// <summary>
    /// Checks health of a specific downstream service via circuit breaker.
    /// </summary>
    [HttpGet("services/{serviceName}/health")]
    public async Task<IActionResult> GetServiceHealth(string serviceName)
    {
        if (!Services.TryGetValue(serviceName.ToLowerInvariant(), out var config))
            return NotFound(new { error = $"Service '{serviceName}' not found" });

        var isHealthy = await _circuitBreaker.IsServiceHealthyAsync(config.Name);
        return Ok(new
        {
            service = config.Name,
            healthy = isHealthy,
            checkedAt = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Checks health of all downstream services.
    /// </summary>
    [HttpGet("services/health")]
    public async Task<IActionResult> GetAllServicesHealth()
    {
        var results = new List<object>();
        foreach (var (key, config) in Services)
        {
            var isHealthy = await _circuitBreaker.IsServiceHealthyAsync(config.Name);
            results.Add(new
            {
                service = config.Name,
                healthy = isHealthy,
                checkedAt = DateTime.UtcNow
            });
        }
        return Ok(results);
    }

    /// <summary>
    /// Proxy a GET request to a downstream service endpoint.
    /// </summary>
    [HttpGet("proxy/{serviceName}/{**path}")]
    public async Task<IActionResult> ProxyGet(string serviceName, string path)
    {
        if (!Services.TryGetValue(serviceName.ToLowerInvariant(), out var config))
            return NotFound(new { error = $"Service '{serviceName}' not found" });

        try
        {
            var client = _circuitBreaker.GetClient(config.Name);
            var response = await client.GetAsync($"/api/{path}");
            var content = await response.Content.ReadAsStringAsync();

            return StatusCode((int)response.StatusCode, content);
        }
        catch (Polly.CircuitBreaker.BrokenCircuitException)
        {
            _logger.LogWarning("Circuit open for {Service}", config.Name);
            return StatusCode(503, new { error = $"Service '{config.Name}' is temporarily unavailable (circuit open)" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error proxying to {Service}", config.Name);
            return StatusCode(502, new { error = $"Error reaching service '{config.Name}'" });
        }
    }

    /// <summary>
    /// Proxy a POST request to a downstream service endpoint.
    /// </summary>
    [HttpPost("proxy/{serviceName}/{**path}")]
    public async Task<IActionResult> ProxyPost(string serviceName, string path)
    {
        if (!Services.TryGetValue(serviceName.ToLowerInvariant(), out var config))
            return NotFound(new { error = $"Service '{serviceName}' not found" });

        try
        {
            var client = _circuitBreaker.GetClient(config.Name);
            using var reader = new StreamReader(Request.Body);
            var body = await reader.ReadToEndAsync();
            var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/api/{path}", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            return StatusCode((int)response.StatusCode, responseContent);
        }
        catch (Polly.CircuitBreaker.BrokenCircuitException)
        {
            return StatusCode(503, new { error = $"Service '{config.Name}' is temporarily unavailable (circuit open)" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error proxying to {Service}", config.Name);
            return StatusCode(502, new { error = $"Error reaching service '{config.Name}'" });
        }
    }

    private record ServiceConfig(string Name, int Port, string Description);
}
