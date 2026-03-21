using MassTransit;
using Microsoft.Extensions.Logging;

namespace ConfigService.Infrastructure.Messaging;

public record ConfigurationUpdatedMessage
{
    public string EntityType { get; init; } = string.Empty;
    public string EntityId { get; init; } = string.Empty;
    public string Action { get; init; } = string.Empty;
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}

public class ConfigurationUpdatedConsumer(ILogger<ConfigurationUpdatedConsumer> logger) : IConsumer<ConfigurationUpdatedMessage>
{
    public Task Consume(ConsumeContext<ConfigurationUpdatedMessage> context)
    {
        var msg = context.Message;
        logger.LogInformation("Received config update: {EntityType}/{EntityId} - {Action} at {Timestamp}",
            msg.EntityType, msg.EntityId, msg.Action, msg.Timestamp);
        return Task.CompletedTask;
    }
}
