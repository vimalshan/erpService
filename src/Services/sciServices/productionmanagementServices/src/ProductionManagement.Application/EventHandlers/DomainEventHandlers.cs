using MediatR;
using Microsoft.Extensions.Logging;
using ProductionManagement.Application.Interfaces;
using ProductionManagement.Domain.Events;

namespace ProductionManagement.Application.EventHandlers;

public class ProductionPlantCreatedEventHandler : INotificationHandler<ProductionPlantCreatedEvent>
{
    private readonly ILogger<ProductionPlantCreatedEventHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public ProductionPlantCreatedEventHandler(ILogger<ProductionPlantCreatedEventHandler> logger, IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(ProductionPlantCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: ProductionPlant created - {PlantId} ({PlantName})",
            notification.Plant.ProductionPlantId, notification.Plant.PlantName);

        await _messagePublisher.PublishAsync("production.events", "production.plant.created",
            new { notification.Plant.ProductionPlantId, notification.Plant.PlantName, notification.OccurredOn },
            cancellationToken);
    }
}

public class ProductionPlantUpdatedEventHandler : INotificationHandler<ProductionPlantUpdatedEvent>
{
    private readonly ILogger<ProductionPlantUpdatedEventHandler> _logger;

    public ProductionPlantUpdatedEventHandler(ILogger<ProductionPlantUpdatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ProductionPlantUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: ProductionPlant updated - {PlantId}", notification.Plant.ProductionPlantId);
        return Task.CompletedTask;
    }
}

public class ProductionPlanCreatedEventHandler : INotificationHandler<ProductionPlanCreatedEvent>
{
    private readonly ILogger<ProductionPlanCreatedEventHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public ProductionPlanCreatedEventHandler(ILogger<ProductionPlanCreatedEventHandler> logger, IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(ProductionPlanCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: ProductionPlan created - Plant={PlantId}, Item={ItemId}",
            notification.Plan.ProductionPlantId, notification.Plan.SciItemId);

        await _messagePublisher.PublishAsync("production.events", "production.plan.created",
            new { notification.Plan.ProductionPlantId, notification.Plan.SciItemId, notification.Plan.QtyPerDay, notification.OccurredOn },
            cancellationToken);
    }
}

public class ProductionPlanClosedEventHandler : INotificationHandler<ProductionPlanClosedEvent>
{
    private readonly ILogger<ProductionPlanClosedEventHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public ProductionPlanClosedEventHandler(ILogger<ProductionPlanClosedEventHandler> logger, IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(ProductionPlanClosedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: ProductionPlan closed - Plant={PlantId}, Item={ItemId}",
            notification.Plan.ProductionPlantId, notification.Plan.SciItemId);

        await _messagePublisher.PublishAsync("production.events", "production.plan.closed",
            new { notification.Plan.ProductionPlantId, notification.Plan.SciItemId, notification.OccurredOn },
            cancellationToken);
    }
}
