using HRService.Infrastructure.MessageBroker;
using Microsoft.AspNetCore.Mvc;

namespace HRService.API.Controllers;

public record HRTestMessage(string Message, DateTime Timestamp, string Source);

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

    /// <summary>Get RabbitMQ connection configuration</summary>
    [HttpGet("status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetStatus()
    {
        var rabbit = _configuration.GetSection("RabbitMQ");
        return Ok(new
        {
            status = "configured",
            host = rabbit["Host"] ?? "localhost",
            port = rabbit.GetValue<int>("Port", 5672),
            virtualHost = rabbit["VirtualHost"] ?? "/",
            enabled = rabbit.GetValue<bool>("Enabled", false)
        });
    }

    /// <summary>Publish a test message to RabbitMQ</summary>
    [HttpPost("publish")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> PublishTestMessage(CancellationToken cancellationToken)
    {
        var message = new HRTestMessage(
            Message: "HRService RabbitMQ connectivity test",
            Timestamp: DateTime.UtcNow,
            Source: "HRService.API");
        try
        {
            await _messageBroker.PublishMessageAsync("hr-events", "hr.test", message, cancellationToken);
            _logger.LogInformation("Test message published to RabbitMQ at {Timestamp}", message.Timestamp);
            return Ok(new
            {
                status = "published",
                message = message.Message,
                timestamp = message.Timestamp,
                exchange = "hr-events",
                routingKey = "hr.test"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish test message to RabbitMQ");
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new
            {
                status = "unavailable",
                error = ex.Message,
                hint = "Ensure RabbitMQ is running: docker run -d -p 5672:5672 -p 15672:15672 rabbitmq:3-management"
            });
        }
    }
}
