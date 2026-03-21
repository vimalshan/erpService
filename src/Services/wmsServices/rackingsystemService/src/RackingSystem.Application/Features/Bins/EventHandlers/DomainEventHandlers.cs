using MediatR;
using Microsoft.Extensions.Logging;
using RackingSystem.Application.Common.Interfaces;
using RackingSystem.Domain.Events;

namespace RackingSystem.Application.Features.Bins.EventHandlers;

public sealed class BinStatusChangedEventHandler : INotificationHandler<BinStatusChangedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<BinStatusChangedEventHandler> _logger;

    public BinStatusChangedEventHandler(IMessagePublisher publisher, ILogger<BinStatusChangedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(BinStatusChangedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Bin {BinId} status changed from {Previous} to {New}",
            notification.BinId, notification.PreviousStatus, notification.NewStatus);

        try
        {
            await _publisher.PublishAsync(
                "racking.exchange",
                "bin.status.changed",
                new { notification.BinId, notification.PreviousStatus, notification.NewStatus, Timestamp = DateTime.UtcNow },
                ct);
        }
        catch (Exception ex)
        {
            // RabbitMQ unavailable — log and continue; the status change is already persisted.
            _logger.LogWarning(ex, "Failed to publish BinStatusChanged event for Bin {BinId}. Message broker may be unavailable.", notification.BinId);
        }
    }
}

public sealed class RackCreatedEventHandler : INotificationHandler<RackCreatedEvent>
{
    private readonly ILogger<RackCreatedEventHandler> _logger;
    public RackCreatedEventHandler(ILogger<RackCreatedEventHandler> logger) => _logger = logger;

    public Task Handle(RackCreatedEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("Rack created: Id={RackId}, Code={Code}, ZoneId={ZoneId}",
            notification.Rack.Id, notification.Rack.Code, notification.Rack.ZoneId);
        return Task.CompletedTask;
    }
}
