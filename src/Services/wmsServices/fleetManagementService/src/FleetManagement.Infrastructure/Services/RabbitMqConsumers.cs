using FleetManagement.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FleetManagement.Infrastructure.Services;

// Message types
public record VehicleMaintenanceMessage(int VehicleId, string MaintenanceType, DateTime ScheduledDate);
public record TripCompletedMessage(int TripId, string TripNumber, int VehicleId, int DriverId);

// Concrete consumers
public class MaintenanceScheduleConsumer(IConfiguration config, ILogger<MaintenanceScheduleConsumer> logger, IServiceScopeFactory scopeFactory)
    : RabbitMqConsumerBase<VehicleMaintenanceMessage>(config, logger)
{
    protected override string QueueName => "fleet.maintenance.schedule";
    protected override string ExchangeName => "fleet.events";
    protected override string RoutingKey => "fleet.maintenance.#";

    protected override async Task HandleMessageAsync(VehicleMaintenanceMessage message, CancellationToken ct)
    {
        using var scope = scopeFactory.CreateScope();
        var publisher = scope.ServiceProvider.GetRequiredService<IMessagePublisher>();
        logger.LogInformation("Processing maintenance schedule for Vehicle {VehicleId}: {Type}", message.VehicleId, message.MaintenanceType);
        await publisher.PublishAsync("fleet.notifications", "fleet.maintenance.processed",
            new { message.VehicleId, message.MaintenanceType, ProcessedAt = DateTime.UtcNow }, ct);
    }
}

public class TripCompletedConsumer(IConfiguration config, ILogger<TripCompletedConsumer> logger, IServiceScopeFactory scopeFactory)
    : RabbitMqConsumerBase<TripCompletedMessage>(config, logger)
{
    protected override string QueueName => "fleet.trip.completed";
    protected override string ExchangeName => "fleet.events";
    protected override string RoutingKey => "fleet.trip.completed";

    protected override async Task HandleMessageAsync(TripCompletedMessage message, CancellationToken ct)
    {
        using var scope = scopeFactory.CreateScope();
        var publisher = scope.ServiceProvider.GetRequiredService<IMessagePublisher>();
        logger.LogInformation("Trip {TripNumber} completed for Vehicle {VehicleId}", message.TripNumber, message.VehicleId);
        await publisher.PublishAsync("fleet.notifications", "fleet.trip.completed.processed",
            new { message.TripId, message.TripNumber, ProcessedAt = DateTime.UtcNow }, ct);
    }
}
