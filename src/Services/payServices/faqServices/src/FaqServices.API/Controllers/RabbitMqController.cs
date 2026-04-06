using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace FaqServices.API.Controllers;

/// <summary>Test message published to RabbitMQ</summary>
public record FaqTestMessage(string Message, DateTime Timestamp, string Source);

/// <summary>
/// RabbitMQ connectivity test endpoints
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RabbitMqController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<RabbitMqController> _logger;

    public RabbitMqController(IConfiguration configuration, ILogger<RabbitMqController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>Get RabbitMQ connection configuration</summary>
    [HttpGet("status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetStatus()
    {
        var rabbitMq = _configuration.GetSection("RabbitMQ");
        return Ok(new
        {
            status = "configured",
            host = rabbitMq["HostName"] ?? "localhost",
            port = rabbitMq.GetValue<int>("Port", 5672),
            virtualHost = rabbitMq["VirtualHost"] ?? "/"
        });
    }

    /// <summary>Publish a test message to RabbitMQ</summary>
    [HttpPost("publish")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> PublishTestMessage(CancellationToken cancellationToken)
    {
        var message = new FaqTestMessage(
            Message: "FaqServices RabbitMQ connectivity test",
            Timestamp: DateTime.UtcNow,
            Source: "FaqServices.API");

        try
        {
            var rabbitMq = _configuration.GetSection("RabbitMQ");
            var factory = new ConnectionFactory
            {
                HostName = rabbitMq["HostName"] ?? "localhost",
                Port = rabbitMq.GetValue<int>("Port", 5672),
                UserName = rabbitMq["UserName"] ?? "guest",
                Password = rabbitMq["Password"] ?? "guest",
                VirtualHost = rabbitMq["VirtualHost"] ?? "/"
            };

            await using var connection = await factory.CreateConnectionAsync(cancellationToken);
            await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            const string queueName = "faq-test";
            await channel.QueueDeclareAsync(queueName, durable: true, exclusive: false,
                autoDelete: false, cancellationToken: cancellationToken);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queueName,
                body: body, cancellationToken: cancellationToken);

            _logger.LogInformation("Test message published to RabbitMQ at {Timestamp}", message.Timestamp);
            return Ok(new
            {
                status = "published",
                message = message.Message,
                timestamp = message.Timestamp,
                queue = queueName
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
