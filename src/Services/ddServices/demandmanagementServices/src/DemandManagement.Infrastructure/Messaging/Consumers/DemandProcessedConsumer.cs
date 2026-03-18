using MassTransit;
using DemandManagement.Infrastructure.Events;
using Microsoft.Extensions.Logging;

namespace DemandManagement.Infrastructure.Messaging.Consumers;

public class DemandProcessedConsumer : IConsumer<DemandProcessedIntegrationEvent>
{
    private readonly ILogger<DemandProcessedConsumer> _logger;

    public DemandProcessedConsumer(ILogger<DemandProcessedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<DemandProcessedIntegrationEvent> context)
    {
        _logger.LogInformation("Demand {DemandId} processed with status {Status}", 
            context.Message.DemandId, context.Message.Status);
        return Task.CompletedTask;
    }
}
