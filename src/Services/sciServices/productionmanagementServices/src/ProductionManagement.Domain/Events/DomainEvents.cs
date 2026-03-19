using ProductionManagement.Domain.Common;
using ProductionManagement.Domain.Entities;

namespace ProductionManagement.Domain.Events;

public class ProductionPlantCreatedEvent : IDomainEvent
{
    public ProductionPlant Plant { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public ProductionPlantCreatedEvent(ProductionPlant plant) => Plant = plant;
}

public class ProductionPlantUpdatedEvent : IDomainEvent
{
    public ProductionPlant Plant { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public ProductionPlantUpdatedEvent(ProductionPlant plant) => Plant = plant;
}

public class ProductionPlanCreatedEvent : IDomainEvent
{
    public ProductionPlan Plan { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public ProductionPlanCreatedEvent(ProductionPlan plan) => Plan = plan;
}

public class ProductionPlanUpdatedEvent : IDomainEvent
{
    public ProductionPlan Plan { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public ProductionPlanUpdatedEvent(ProductionPlan plan) => Plan = plan;
}

public class ProductionPlanClosedEvent : IDomainEvent
{
    public ProductionPlan Plan { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public ProductionPlanClosedEvent(ProductionPlan plan) => Plan = plan;
}
