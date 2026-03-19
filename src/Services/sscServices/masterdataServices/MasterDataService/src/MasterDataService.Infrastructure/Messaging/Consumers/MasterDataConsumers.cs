using MassTransit;
using Microsoft.Extensions.Logging;

namespace MasterDataService.Infrastructure.Messaging.Consumers;

// Message contracts
public record MasterDataUpdatedMessage
{
    public string EntityType { get; init; } = null!;
    public string EntityId { get; init; } = null!;
    public string Action { get; init; } = null!;
    public DateTime Timestamp { get; init; } = DateTime.UtcNow;
}

public record MasterDataSyncRequestMessage
{
    public string EntityType { get; init; } = null!;
    public DateTime? SyncFrom { get; init; }
}

// Consumers
public class MasterDataUpdatedConsumer(ILogger<MasterDataUpdatedConsumer> logger)
    : IConsumer<MasterDataUpdatedMessage>
{
    public Task Consume(ConsumeContext<MasterDataUpdatedMessage> context)
    {
        var msg = context.Message;
        logger.LogInformation("Received MasterDataUpdated: {EntityType} {EntityId} {Action} at {Timestamp}",
            msg.EntityType, msg.EntityId, msg.Action, msg.Timestamp);
        return Task.CompletedTask;
    }
}

public class MasterDataSyncRequestConsumer(ILogger<MasterDataSyncRequestConsumer> logger)
    : IConsumer<MasterDataSyncRequestMessage>
{
    public Task Consume(ConsumeContext<MasterDataSyncRequestMessage> context)
    {
        var msg = context.Message;
        logger.LogInformation("Received MasterDataSyncRequest: {EntityType}, SyncFrom: {SyncFrom}",
            msg.EntityType, msg.SyncFrom);
        return Task.CompletedTask;
    }
}
