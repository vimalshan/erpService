using MassTransit;
using Microsoft.Extensions.Logging;

namespace CompetencyService.Infrastructure.Messaging.Consumers;

public record CompetencyAssignedMessage(decimal EmpSysId, decimal CompetencyId, decimal YearId, char Type);
public record CompetencyRemovedMessage(decimal EmpSysId, decimal CompetencyId, decimal YearId);

public class CompetencyAssignedConsumer(ILogger<CompetencyAssignedConsumer> logger)
    : IConsumer<CompetencyAssignedMessage>
{
    public Task Consume(ConsumeContext<CompetencyAssignedMessage> context)
    {
        logger.LogInformation(
            "Received CompetencyAssigned: EmpSysId={EmpId} CompetencyId={CompId}",
            context.Message.EmpSysId, context.Message.CompetencyId);
        // Add cross-service business logic here (e.g., notify appraisal service)
        return Task.CompletedTask;
    }
}

public class CompetencyRemovedConsumer(ILogger<CompetencyRemovedConsumer> logger)
    : IConsumer<CompetencyRemovedMessage>
{
    public Task Consume(ConsumeContext<CompetencyRemovedMessage> context)
    {
        logger.LogInformation(
            "Received CompetencyRemoved: EmpSysId={EmpId} CompetencyId={CompId}",
            context.Message.EmpSysId, context.Message.CompetencyId);
        return Task.CompletedTask;
    }
}
