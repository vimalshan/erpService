using System;
using System.Threading.Tasks;
using AppraisalService.Infrastructure.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AppraisalService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class RabbitMQTestController : ControllerBase
{
    private readonly IMessagePublisher _publisher;

    public RabbitMQTestController(IMessagePublisher publisher)
    {
        _publisher = publisher;
    }

    [HttpPost("publish")]
    public async Task<IActionResult> PublishTestMessage([FromBody] TestMessage message)
    {
        try
        {
            await _publisher.PublishAsync("test_exchange", "test.routing.key", message);
            return Ok(new { success = true, detail = "Message successfully published to RabbitMQ." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, detail = "Failed to connect to RabbitMQ or publish message.", exception = ex.Message });
        }
    }
}

public class TestMessage
{
    public string Subject { get; set; } = "Test Subject";
    public string Content { get; set; } = "This is a test message to RabbitMQ.";
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
