using MediatR;
using Microsoft.Extensions.Logging;
using ProjectService.Domain.Events;
using ProjectService.Domain.Interfaces;

namespace ProjectService.Infrastructure.DomainEventHandlers;

public class ProjectCreatedEventHandler(
    ILogger<ProjectCreatedEventHandler> logger,
    IMessagePublisher publisher)
    : INotificationHandler<ProjectCreatedEvent>
{
    public async Task Handle(ProjectCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: Project {ProjectId} '{ProjectName}' created", notification.ProjectId, notification.ProjectName);
        await publisher.PublishAsync("project-exchange", "project.created", notification, cancellationToken);
    }
}

public class ProjectStatusChangedEventHandler(
    ILogger<ProjectStatusChangedEventHandler> logger,
    IMessagePublisher publisher)
    : INotificationHandler<ProjectStatusChangedEvent>
{
    public async Task Handle(ProjectStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: Project {ProjectId} status changed from {Old} to {New}",
            notification.ProjectId, notification.OldStatus, notification.NewStatus);
        await publisher.PublishAsync("project-exchange", "project.status.changed", notification, cancellationToken);
    }
}

public class ProjectClosedEventHandler(
    ILogger<ProjectClosedEventHandler> logger,
    IMessagePublisher publisher)
    : INotificationHandler<ProjectClosedEvent>
{
    public async Task Handle(ProjectClosedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: Project {ProjectId} closed on {Date}", notification.ProjectId, notification.ClosedDate);
        await publisher.PublishAsync("project-exchange", "project.closed", notification, cancellationToken);
    }
}

public class ProjectHeldEventHandler(
    ILogger<ProjectHeldEventHandler> logger,
    IMessagePublisher publisher)
    : INotificationHandler<ProjectHeldEvent>
{
    public async Task Handle(ProjectHeldEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: Project {ProjectId} put on hold: {Reason}", notification.ProjectId, notification.Reason);
        await publisher.PublishAsync("project-exchange", "project.held", notification, cancellationToken);
    }
}

public class ProjectMemberAddedEventHandler(ILogger<ProjectMemberAddedEventHandler> logger)
    : INotificationHandler<ProjectMemberAddedEvent>
{
    public Task Handle(ProjectMemberAddedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: Member {MemberId} (Employee {EmployeeId}) added to Project {ProjectId}",
            notification.MemberId, notification.EmployeeId, notification.ProjectId);
        return Task.CompletedTask;
    }
}
