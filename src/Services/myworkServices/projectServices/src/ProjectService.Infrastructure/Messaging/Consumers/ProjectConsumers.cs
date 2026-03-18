using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProjectService.Domain.Interfaces;

namespace ProjectService.Infrastructure.Messaging.Consumers;

public class ProjectApprovalMessage
{
    public long ProjectId { get; set; }
    public long ApprovalId { get; set; }
    public char Status { get; set; }
    public string Remarks { get; set; } = null!;
}

public class ProjectApprovalConsumer(
    IServiceProvider serviceProvider,
    ILogger<ProjectApprovalConsumer> logger,
    IConfiguration configuration)
    : RabbitMqConsumerBase<ProjectApprovalMessage>(
        serviceProvider, logger, configuration,
        "project-approval-queue", "project-exchange", "project.approval.#")
{
    protected override async Task HandleMessageAsync(ProjectApprovalMessage message, IServiceScope scope, CancellationToken cancellationToken)
    {
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var project = await unitOfWork.ProjectMains.GetByIdAsync(message.ProjectId, cancellationToken);
        if (project is null) return;

        logger.LogInformation("Processing approval for project {ProjectId}, Status: {Status}", message.ProjectId, message.Status);
    }
}

public class ProjectStatusUpdateMessage
{
    public long ProjectId { get; set; }
    public char NewStatus { get; set; }
    public string? Reason { get; set; }
}

public class ProjectStatusUpdateConsumer(
    IServiceProvider serviceProvider,
    ILogger<ProjectStatusUpdateConsumer> logger,
    IConfiguration configuration)
    : RabbitMqConsumerBase<ProjectStatusUpdateMessage>(
        serviceProvider, logger, configuration,
        "project-status-queue", "project-exchange", "project.status.#")
{
    protected override async Task HandleMessageAsync(ProjectStatusUpdateMessage message, IServiceScope scope, CancellationToken cancellationToken)
    {
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var project = await unitOfWork.ProjectMains.GetByIdAsync(message.ProjectId, cancellationToken);
        if (project is null) return;

        project.ProjStatus = message.NewStatus;
        project.ProjLastModifiedOn = DateTime.UtcNow;
        await unitOfWork.ProjectMains.UpdateAsync(project, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Updated project {ProjectId} status to {Status}", message.ProjectId, message.NewStatus);
    }
}
