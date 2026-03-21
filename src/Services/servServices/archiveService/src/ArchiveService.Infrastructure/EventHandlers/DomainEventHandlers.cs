using ArchiveService.Application.Interfaces;
using ArchiveService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ArchiveService.Infrastructure.EventHandlers;

public class ServiceOrderArchivedEventHandler(
    ILogger<ServiceOrderArchivedEventHandler> logger,
    IMessagePublisher publisher) : INotificationHandler<ServiceOrderArchivedEvent>
{
    public async Task Handle(ServiceOrderArchivedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Service order archived: {SernoDell}, SAP: {SapId}",
            notification.SernoDell, notification.SapId);

        await publisher.PublishAsync("archive-exchange", "order.archived", notification, ct);
    }
}

public class ServiceOrderStatusChangedEventHandler(
    ILogger<ServiceOrderStatusChangedEventHandler> logger,
    IMessagePublisher publisher) : INotificationHandler<ServiceOrderStatusChangedEvent>
{
    public async Task Handle(ServiceOrderStatusChangedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Service order status changed: {SernoDell} -> {NewStatus}",
            notification.SernoDell, notification.NewStatus);

        await publisher.PublishAsync("archive-exchange", "order.status.changed", notification, ct);
    }
}

public class ToolKitArchivedEventHandler(
    ILogger<ToolKitArchivedEventHandler> logger,
    IMessagePublisher publisher) : INotificationHandler<ToolKitArchivedEvent>
{
    public async Task Handle(ToolKitArchivedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Toolkit archived: {KitCode}, Engineer: {EngineerId}",
            notification.KitCode, notification.EngineerId);

        await publisher.PublishAsync("archive-exchange", "toolkit.archived", notification, ct);
    }
}
