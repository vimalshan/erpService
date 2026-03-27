using ApiGateway.API.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ApiGateway.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class GatewayController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ServiceEndpoints _endpoints;
    private readonly ILogger<GatewayController> _logger;

    public GatewayController(
        IHttpClientFactory httpClientFactory,
        IOptions<ServiceEndpoints> endpoints,
        ILogger<GatewayController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _endpoints = endpoints.Value;
        _logger = logger;
    }

    [HttpGet("services")]
    [ProducesResponseType(typeof(IEnumerable<ServiceRouteInfo>), StatusCodes.Status200OK)]
    public IActionResult GetServices()
    {
        var services = new[]
        {
            new ServiceRouteInfo("CashManagement", _endpoints.CashManagement, "/api/v1/cash", "Cash transactions, bank accounts, cheques, reconciliation"),
            new ServiceRouteInfo("CurrencyManagement", _endpoints.CurrencyManagement, "/api/v1/currency", "Currency master, exchange rates, org currency mapping"),
            new ServiceRouteInfo("DealTicketing", _endpoints.DealTicketing, "/api/v1/deals", "Deal batches, deal details, settlements, attachments"),
            new ServiceRouteInfo("LoanManagement", _endpoints.LoanManagement, "/api/v1/loans", "Loans, disbursements, interest, repayment schedules"),
            new ServiceRouteInfo("OrganizationSetup", _endpoints.OrganizationSetup, "/api/v1/organization", "Roles, user mapping, org params, PP limits"),
            new ServiceRouteInfo("EmailNotification", _endpoints.EmailNotification, "/api/v1/email", "Email types, mail access, notifications")
        };

        return Ok(services);
    }

    [HttpGet("services/{serviceName}/health")]
    [ProducesResponseType(typeof(ServiceHealthResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetServiceHealth(string serviceName, CancellationToken ct)
    {
        var all = _endpoints.GetAll();
        if (!all.TryGetValue(serviceName, out var baseUrl))
            return NotFound(new { message = $"Service '{serviceName}' not found." });

        using var client = _httpClientFactory.CreateClient("HealthCheck");
        client.Timeout = TimeSpan.FromSeconds(5);

        try
        {
            var response = await client.GetAsync($"{baseUrl}/health", ct);
            return Ok(new ServiceHealthResult(serviceName, baseUrl, response.IsSuccessStatusCode ? "Healthy" : "Unhealthy"));
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Health check failed for {Service}", serviceName);
            return Ok(new ServiceHealthResult(serviceName, baseUrl, "Unavailable"));
        }
    }
}

public sealed record ServiceRouteInfo(string Name, string BaseUrl, string GatewayPrefix, string Description);
public sealed record ServiceHealthResult(string Name, string BaseUrl, string Status);
