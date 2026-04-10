using Microsoft.AspNetCore.Mvc;

namespace SparshApiGateway.Controllers;

/// <summary>
/// Gateway info and diagnostics controller.
/// </summary>
[ApiController]
[Route("api/gateway")]
public class GatewayController(IConfiguration configuration) : ControllerBase
{
    [HttpGet("info")]
    public IActionResult GetGatewayInfo()
    {
        var services = configuration.GetSection("ServiceDiscovery:Services")
            .GetChildren()
            .Select(s => new { Service = s.Key, Url = s.Value })
            .ToList();

        return Ok(new
        {
            Gateway = "Sparsh API Gateway",
            Version = "1.0.0",
            Timestamp = DateTime.UtcNow,
            ProxyEngines = new[] { "YARP (primary)", "Ocelot (secondary)" },
            Features = new[]
            {
                "JWT Authentication & Authorization",
                "Rate Limiting & Throttling",
                "Circuit Breaker Pattern",
                "Retry with Exponential Backoff",
                "Timeout Handling",
                "Bulkhead Isolation",
                "Correlation ID Tracking",
                "Request/Response Logging",
                "Response Caching",
                "Load Balancing (Round Robin)",
                "Health Checks & Monitoring"
            },
            RegisteredServices = services,
            Endpoints = new
            {
                YARP = new
                {
                    EmployeePride = "/yarp/employee-pride/{path}",
                    MobileApp = "/yarp/mobile-app/{path}",
                    MobileExpense = "/yarp/mobile-expense/{path}",
                    Problem = "/yarp/problem/{path}",
                    Transactional = "/yarp/transactional/{path}"
                },
                Ocelot = new
                {
                    EmployeePride = "/ocelot/employee-pride/{path}",
                    MobileApp = "/ocelot/mobile-app/{path}",
                    MobileExpense = "/ocelot/mobile-expense/{path}",
                    Problem = "/ocelot/problem/{path}",
                    Transactional = "/ocelot/transactional/{path}"
                }
            }
        });
    }

    [HttpGet("services")]
    public IActionResult GetServices()
    {
        var services = configuration.GetSection("ServiceDiscovery:Services")
            .GetChildren()
            .Select(s => new { Service = s.Key, Url = s.Value })
            .ToList();

        return Ok(services);
    }
}
