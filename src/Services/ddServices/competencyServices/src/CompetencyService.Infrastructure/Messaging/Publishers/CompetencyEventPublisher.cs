using MassTransit;
using Microsoft.Extensions.Logging;

namespace CompetencyService.Infrastructure.Messaging.Publishers;

public interface ICompetencyEventPublisher
{
    Task PublishCompetencyCreatedAsync(decimal competencyId, string name, CancellationToken ct = default);
    Task PublishEmpCompetencyAssignedAsync(decimal empSysId, decimal competencyId, CancellationToken ct = default);
}

public record CompetencyCreatedIntegrationEvent(decimal CompetencyId, string Name, DateTime OccurredOn);
public record EmpCompetencyAssignedIntegrationEvent(decimal EmpSysId, decimal CompetencyId, DateTime OccurredOn);

public class CompetencyEventPublisher(IPublishEndpoint publishEndpoint, ILogger<CompetencyEventPublisher> logger)
    : ICompetencyEventPublisher
{
    public async Task PublishCompetencyCreatedAsync(decimal competencyId, string name, CancellationToken ct)
    {
        var @event = new CompetencyCreatedIntegrationEvent(competencyId, name, DateTime.UtcNow);
        await publishEndpoint.Publish(@event, ct);
        logger.LogInformation("Published CompetencyCreatedIntegrationEvent for Id={Id}", competencyId);
    }

    public async Task PublishEmpCompetencyAssignedAsync(decimal empSysId, decimal competencyId, CancellationToken ct)
    {
        var @event = new EmpCompetencyAssignedIntegrationEvent(empSysId, competencyId, DateTime.UtcNow);
        await publishEndpoint.Publish(@event, ct);
        logger.LogInformation("Published EmpCompetencyAssignedIntegrationEvent for EmpId={EmpId}", empSysId);
    }
}

/// <summary>No-op publisher used when RabbitMQ is not available (e.g. local dev without broker).</summary>
public sealed class NullCompetencyEventPublisher : ICompetencyEventPublisher
{
    public Task PublishCompetencyCreatedAsync(decimal competencyId, string name, CancellationToken ct = default)
        => Task.CompletedTask;

    public Task PublishEmpCompetencyAssignedAsync(decimal empSysId, decimal competencyId, CancellationToken ct = default)
        => Task.CompletedTask;
}
