using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GatewayController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public GatewayController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    [HttpGet("services")]
    public IActionResult GetServices()
    {
        var endpoints = _configuration.GetSection("ServiceEndpoints").Get<Dictionary<string, string>>() ?? new();
        return Ok(endpoints.Select(e => new
        {
            name = e.Key,
            baseUrl = e.Value,
            healthUrl = $"{e.Value}/health",
            swaggerUrl = $"{e.Value}/swagger/index.html",
            graphqlUrl = $"{e.Value}/graphql"
        }));
    }

    [Authorize]
    [HttpGet("services/{serviceName}/health")]
    public async Task<IActionResult> GetServiceHealth(string serviceName, CancellationToken ct)
    {
        var endpoints = _configuration.GetSection("ServiceEndpoints").Get<Dictionary<string, string>>() ?? new();
        if (!endpoints.TryGetValue(serviceName, out var baseUrl))
            return NotFound(new { message = $"Service '{serviceName}' not found." });

        var client = _httpClientFactory.CreateClient("HealthCheckClient");
        try
        {
            var response = await client.GetAsync($"{baseUrl}/health", ct);
            var content = await response.Content.ReadAsStringAsync(ct);
            return StatusCode((int)response.StatusCode, System.Text.Json.JsonSerializer.Deserialize<object>(content));
        }
        catch (Exception ex)
        {
            return StatusCode(503, new { service = serviceName, status = "Unavailable", error = ex.Message });
        }
    }
}
