using MediatR;
using Microsoft.Extensions.Logging;
using VehicleTracking.Domain.Events;
using VehicleTracking.Domain.Interfaces;

namespace VehicleTracking.Application.Vehicles.EventHandlers;

public class VehicleRegisteredEventHandler(ILogger<VehicleRegisteredEventHandler> logger, IMessagePublisher publisher)
    : INotificationHandler<VehicleRegisteredEvent>
{
    public async Task Handle(VehicleRegisteredEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Vehicle registered: {RegNum}", notification.Vehicle.GetRegistrationNumber());
        await publisher.PublishAsync("vehicle-tracking", "vehicle.registered",
            new { notification.Vehicle.SerialNumber, Registration = notification.Vehicle.GetRegistrationNumber().ToString() }, ct);
    }
}

public class VehicleStageUpdatedEventHandler(ILogger<VehicleStageUpdatedEventHandler> logger, IMessagePublisher publisher)
    : INotificationHandler<VehicleStageUpdatedEvent>
{
    public async Task Handle(VehicleStageUpdatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Vehicle stage updated: Tracking={TrackingNumber}, Stage={StageCode}",
            notification.TrackingNumber, notification.StageCode);
        await publisher.PublishAsync("vehicle-tracking", "vehicle.stage.updated",
            new { notification.TrackingNumber, notification.StageCode }, ct);
    }
}

public class VehicleTransactionCreatedEventHandler(ILogger<VehicleTransactionCreatedEventHandler> logger)
    : INotificationHandler<VehicleTransactionCreatedEvent>
{
    public Task Handle(VehicleTransactionCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Vehicle transaction created: Tracking={TrackingNumber}", notification.TrackingNumber);
        return Task.CompletedTask;
    }
}

public class DecisionMadeEventHandler(ILogger<DecisionMadeEventHandler> logger, IMessagePublisher publisher)
    : INotificationHandler<DecisionMadeEvent>
{
    public async Task Handle(DecisionMadeEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Decision made: Tracking={TrackingNumber}, Purpose={PurposeCode}, Stage={StageCode}, Decision={Decision}",
            notification.TrackingNumber, notification.PurposeCode, notification.StageCode, notification.Decision);
        await publisher.PublishAsync("vehicle-tracking", "vehicle.decision.made",
            new { notification.TrackingNumber, notification.PurposeCode, notification.StageCode, notification.Decision }, ct);
    }
}
