using FleetManagement.Application.Interfaces;
using FleetManagement.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FleetManagement.Infrastructure.Services;

public class VehicleStatusChangedHandler(IMessagePublisher publisher, ILogger<VehicleStatusChangedHandler> logger) : INotificationHandler<VehicleStatusChangedEvent>
{
    public async Task Handle(VehicleStatusChangedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Vehicle {VehicleId} status changed to {Status}", notification.VehicleId, notification.NewStatus);
        await publisher.PublishAsync("fleet.events", "fleet.vehicle.status",
            new { notification.VehicleId, Status = notification.NewStatus.ToString(), Timestamp = DateTime.UtcNow }, ct);
    }
}

public class TripStatusChangedHandler(IMessagePublisher publisher, ILogger<TripStatusChangedHandler> logger) : INotificationHandler<TripStatusChangedEvent>
{
    public async Task Handle(TripStatusChangedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Trip {TripId} status changed to {Status}", notification.TripId, notification.NewStatus);
        await publisher.PublishAsync("fleet.events", $"fleet.trip.{notification.NewStatus.ToString().ToLowerInvariant()}",
            new { notification.TripId, Status = notification.NewStatus.ToString(), Timestamp = DateTime.UtcNow }, ct);
    }
}

public class TripCreatedHandler(IMessagePublisher publisher, ILogger<TripCreatedHandler> logger) : INotificationHandler<TripCreatedEvent>
{
    public async Task Handle(TripCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Trip {TripNumber} created", notification.TripNumber);
        await publisher.PublishAsync("fleet.events", "fleet.trip.created", notification, ct);
    }
}

public class MaintenanceLoggedHandler(IMessagePublisher publisher, ILogger<MaintenanceLoggedHandler> logger) : INotificationHandler<MaintenanceLoggedEvent>
{
    public async Task Handle(MaintenanceLoggedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Maintenance logged for Vehicle {VehicleId}: {Type}", notification.VehicleId, notification.MaintenanceType);
        await publisher.PublishAsync("fleet.events", "fleet.maintenance.logged", notification, ct);
    }
}

public class FuelLoggedHandler(IMessagePublisher publisher, ILogger<FuelLoggedHandler> logger) : INotificationHandler<FuelLoggedEvent>
{
    public async Task Handle(FuelLoggedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Fuel logged for Vehicle {VehicleId}: {Gallons}gal ${Cost}", notification.VehicleId, notification.Gallons, notification.Cost);
        await publisher.PublishAsync("fleet.events", "fleet.fuel.logged", notification, ct);
    }
}
