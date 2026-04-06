using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeService.API.Controllers;

/// <summary>Test message contract published to RabbitMQ</summary>
public record EmployeeTestMessage(string Message, DateTime Timestamp, string Source);

/// <summary>
/// RabbitMQ connectivity test endpoints
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
public class RabbitMqController : ControllerBase
{
    private readonly IBus _bus;
    private readonly ILogger<RabbitMqController> _logger;

    public RabbitMqController(IBus bus, ILogger<RabbitMqController> logger)
    {
        _bus = bus;
        _logger = logger;
    }

    /// <summary>
    /// Publish a test message to RabbitMQ and verify connectivity
    /// </summary>
    [HttpPost("publish")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> PublishTestMessage(CancellationToken cancellationToken)
    {
        var message = new EmployeeTestMessage(
            Message: "EmployeeService RabbitMQ connectivity test",
            Timestamp: DateTime.UtcNow,
            Source: "EmployeeService.API");

        try
        {
            await _bus.Publish(message, cancellationToken);
            _logger.LogInformation("Test message published to RabbitMQ at {Timestamp}", message.Timestamp);

            return Ok(new
            {
                status = "published",
                message = message.Message,
                timestamp = message.Timestamp,
                exchange = "employee-test-message"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish test message to RabbitMQ");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new
            {
                status = "unavailable",
                error = ex.Message,
                hint = "Ensure RabbitMQ is running on localhost:5672 (or via Docker: docker run -d -p 5672:5672 -p 15672:15672 rabbitmq:3-management)"
            });
        }
    }

    /// <summary>
    /// Get RabbitMQ bus status
    /// </summary>
    [HttpGet("status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetStatus()
    {
        var busStatus = _bus.GetProbeResult();
        return Ok(new
        {
            status = "connected",
            busType = _bus.GetType().Name,
            address = _bus.Address?.ToString()
        });
    }
}
