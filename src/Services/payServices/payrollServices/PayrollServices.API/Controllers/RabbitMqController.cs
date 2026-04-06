using Microsoft.AspNetCore.Mvc;
using PayrollServices.Infrastructure.Messaging;

namespace PayrollServices.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RabbitMqController : ControllerBase
{
    private readonly IMessageBrokerService _messageBroker;
    private readonly IConfiguration _configuration;
    private readonly ILogger<RabbitMqController> _logger;

    public RabbitMqController(
        IMessageBrokerService messageBroker,
        IConfiguration configuration,
        ILogger<RabbitMqController> logger)
    {
        _messageBroker = messageBroker;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Get RabbitMQ connection status and configuration
    /// </summary>
    [HttpGet("status")]
    public IActionResult GetStatus()
    {
        var rabbit = _configuration.GetSection("RabbitMQ");
        return Ok(new
        {
            host = rabbit["HostName"] ?? "localhost",
            port = rabbit["Port"] ?? "5672",
            virtualHost = rabbit["VirtualHost"] ?? "/",
            enabled = rabbit["Enabled"] ?? "false",
            service = "PayrollServices"
        });
    }

    /// <summary>
    /// Publish a test message to the payroll-events exchange
    /// </summary>
    [HttpPost("publish")]
    public async Task<IActionResult> PublishTestMessage(CancellationToken cancellationToken)
    {
        try
        {
            var message = new PayrollTestMessage(
                Message: "Test message from PayrollServices",
                Timestamp: DateTime.UtcNow,
                Source: "PayrollServices.API"
            );

            await _messageBroker.PublishMessageAsync("payroll-events", "payroll.test", message, cancellationToken);

            return Ok(new
            {
                status = "published",
                message = message.Message,
                timestamp = message.Timestamp,
                exchange = "payroll-events",
                routingKey = "payroll.test"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish test message to RabbitMQ");
            return StatusCode(503, new
            {
                status = "unavailable",
                error = ex.Message,
                hint = "Ensure RabbitMQ is running: docker run -d -p 5672:5672 -p 15672:15672 rabbitmq:3-management"
            });
        }
    }
}

record PayrollTestMessage(string Message, DateTime Timestamp, string Source);
