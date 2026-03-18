using MediatR;
using MassTransit;
using LovService.Application.Events;
using LovService.Domain.Events;

namespace LovService.Infrastructure.MessageBus;

/// <summary>
/// Publishes integration events to RabbitMQ when domain events fire.
/// </summary>
public sealed class LovMasterCreatedEventHandler(IPublishEndpoint bus)
    : INotificationHandler<LovMasterCreatedEvent>
{
    public Task Handle(LovMasterCreatedEvent notification, CancellationToken ct)
        => bus.Publish(new LovMasterCreatedIntegrationEvent(
            notification.LovMaster.LovId,
            notification.LovMaster.LovTypeId,
            notification.LovMaster.LovName,
            notification.LovMaster.LovCreatedBy), ct);
}

public sealed class LovMasterUpdatedEventHandler(IPublishEndpoint bus)
    : INotificationHandler<LovMasterUpdatedEvent>
{
    public Task Handle(LovMasterUpdatedEvent notification, CancellationToken ct)
        => bus.Publish(new LovMasterUpdatedIntegrationEvent(
            notification.LovMaster.LovId,
            notification.LovMaster.LovName,
            notification.LovMaster.LovUpdatedBy), ct);
}

public sealed class LovMasterDeletedEventHandler(IPublishEndpoint bus)
    : INotificationHandler<LovMasterDeletedEvent>
{
    public Task Handle(LovMasterDeletedEvent notification, CancellationToken ct)
        => bus.Publish(new LovMasterDeletedIntegrationEvent(
            notification.LovId, notification.LovTypeId), ct);
}
