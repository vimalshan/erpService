using System;
using Microsoft.Extensions.Logging;

namespace AdminService.Infrastructure.Messaging;

/// <summary>
/// Base class for RabbitMQ message consumers
/// </summary>
public abstract class MessageConsumerBase
{
    protected readonly ILogger<MessageConsumerBase> Logger;

    protected MessageConsumerBase(ILogger<MessageConsumerBase> logger)
    {
        Logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Handle the message
    /// </summary>
    public abstract Task HandleAsync<T>(T message, CancellationToken cancellationToken = default);
}

/// <summary>
/// Consumer for AdminUnit domain events
/// </summary>
public class AdminUnitEventConsumer : MessageConsumerBase
{
    public AdminUnitEventConsumer(ILogger<AdminUnitEventConsumer> logger) : base(logger)
    {
    }

    public override async Task HandleAsync<T>(T message, CancellationToken cancellationToken = default)
    {
        Logger.LogInformation("Processing admin unit event: {EventType}", typeof(T).Name);
        await Task.CompletedTask;
    }
}
