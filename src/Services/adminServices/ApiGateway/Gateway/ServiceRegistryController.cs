using ApiGateway.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Gateway;

[ApiController]
[Route("api/gateway/services")]
public class ServiceRegistryController : ControllerBase
{
    private readonly GatewayConfiguration _config;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ServiceRegistryController> _logger;

    public ServiceRegistryController(
        GatewayConfiguration config,
        IHttpClientFactory httpClientFactory,
        ILogger<ServiceRegistryController> logger)
    {
        _config = config;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    /// <summary>
    /// List all registered downstream services
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public IActionResult GetAllServices()
    {
        var services = _config.Services.Select(s => new
        {
            s.Name,
            url = $"{s.BaseUrl}:{s.Port}",
            s.HealthCheckPath,
            s.TimeoutSeconds,
            s.RequiresAuthentication,
            s.CachingEnabled,
            proxyRoute = $"/{s.Name.ToLower()}/{{**path}}"
        });

        return Ok(new
        {
            gateway = _config.ServiceName,
            version = _config.Version,
            serviceCount = _config.Services.Count,
            services
        });
    }

    /// <summary>
    /// Check health of a specific downstream service
    /// </summary>
    [HttpGet("{serviceName}/health")]
    [AllowAnonymous]
    public async Task<IActionResult> CheckServiceHealth(string serviceName)
    {
        var service = _config.Services.FirstOrDefault(
            s => s.Name.Equals(serviceName, StringComparison.OrdinalIgnoreCase));

        if (service == null)
            return NotFound(new { error = $"Service '{serviceName}' not registered" });

        try
        {
            var client = _httpClientFactory.CreateClient(service.Name);
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var response = await client.GetAsync(service.HealthCheckPath, cts.Token);

            return Ok(new
            {
                service = service.Name,
                status = response.IsSuccessStatusCode ? "Healthy" : "Unhealthy",
                statusCode = (int)response.StatusCode,
                url = $"{service.BaseUrl}:{service.Port}"
            });
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Health check failed for {Service}", serviceName);
            return Ok(new
            {
                service = service.Name,
                status = "Unreachable",
                error = ex.Message,
                url = $"{service.BaseUrl}:{service.Port}"
            });
        }
    }

    /// <summary>
    /// Check health of all downstream services
    /// </summary>
    [HttpGet("health")]
    [AllowAnonymous]
    public async Task<IActionResult> CheckAllServicesHealth()
    {
        var results = new List<object>();

        foreach (var service in _config.Services)
        {
            try
            {
                var client = _httpClientFactory.CreateClient(service.Name);
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                var response = await client.GetAsync(service.HealthCheckPath, cts.Token);

                results.Add(new
                {
                    service = service.Name,
                    status = response.IsSuccessStatusCode ? "Healthy" : "Unhealthy",
                    statusCode = (int)response.StatusCode
                });
            }
            catch (Exception ex)
            {
                results.Add(new
                {
                    service = service.Name,
                    status = "Unreachable",
                    statusCode = 0,
                    error = ex.Message
                });
            }
        }

        return Ok(new
        {
            timestamp = DateTime.UtcNow,
            services = results
        });
    }
}
