using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProductionManagement.Application.DTOs;

namespace ProductionManagement.Infrastructure.Messaging.Consumers;

public class ProductionPlanUpdatedConsumer : RabbitMqConsumerBase<ProductionPlanDto>
{
    protected override string QueueName => "production.plan.updated";
    protected override string ExchangeName => "production.events";
    protected override string RoutingKey => "production.plan.updated";

    public ProductionPlanUpdatedConsumer(
        IConfiguration configuration,
        ILogger<ProductionPlanUpdatedConsumer> logger,
        IServiceScopeFactory scopeFactory)
        : base(configuration, logger, scopeFactory) { }

    protected override Task HandleMessageAsync(ProductionPlanDto message, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<ProductionPlanUpdatedConsumer>>();
        logger.LogInformation("Received ProductionPlanUpdated: Plant={PlantId}, Item={ItemId}, Qty={Qty}",
            message.ProductionPlantId, message.SciItemId, message.QtyPerDay);
        return Task.CompletedTask;
    }
}

public class ProductionPlantCreatedConsumer : RabbitMqConsumerBase<ProductionPlantDto>
{
    protected override string QueueName => "production.plant.created";
    protected override string ExchangeName => "production.events";
    protected override string RoutingKey => "production.plant.created";

    public ProductionPlantCreatedConsumer(
        IConfiguration configuration,
        ILogger<ProductionPlantCreatedConsumer> logger,
        IServiceScopeFactory scopeFactory)
        : base(configuration, logger, scopeFactory) { }

    protected override Task HandleMessageAsync(ProductionPlantDto message, IServiceProvider serviceProvider, CancellationToken cancellationToken)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<ProductionPlantCreatedConsumer>>();
        logger.LogInformation("Received ProductionPlantCreated: PlantId={PlantId}, Name={Name}",
            message.ProductionPlantId, message.PlantName);
        return Task.CompletedTask;
    }
}
