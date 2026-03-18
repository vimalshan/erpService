using MediatR;
using Microsoft.Extensions.Logging;
using TeamServices.Application.Interfaces;
using TeamServices.Domain.Events;

namespace TeamServices.Application.EventHandlers;

public class TeamCreatedEventHandler : INotificationHandler<TeamCreatedEvent>
{
    private readonly ILogger<TeamCreatedEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public TeamCreatedEventHandler(ILogger<TeamCreatedEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(TeamCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Team created - {TeamId}: {TeamName}", notification.TeamId, notification.TeamName);
        await _publisher.PublishAsync("team.events", "team.created",
            new { notification.TeamId, notification.TeamName, Timestamp = DateTime.UtcNow }, cancellationToken);
    }
}

public class TeamUpdatedEventHandler : INotificationHandler<TeamUpdatedEvent>
{
    private readonly ILogger<TeamUpdatedEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public TeamUpdatedEventHandler(ILogger<TeamUpdatedEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(TeamUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Team updated - {TeamId}: {OldName} -> {NewName}",
            notification.TeamId, notification.OldName, notification.NewName);
        await _publisher.PublishAsync("team.events", "team.updated",
            new { notification.TeamId, notification.OldName, notification.NewName, Timestamp = DateTime.UtcNow }, cancellationToken);
    }
}

public class TeamMemberAddedEventHandler : INotificationHandler<TeamMemberAddedEvent>
{
    private readonly ILogger<TeamMemberAddedEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public TeamMemberAddedEventHandler(ILogger<TeamMemberAddedEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(TeamMemberAddedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Member added - Team {TeamId}, Employee {EmployeeSysId}",
            notification.TeamId, notification.EmployeeSysId);
        await _publisher.PublishAsync("team.events", "team.member.added",
            new { notification.TeamId, notification.EmployeeSysId, Action = "Added", Timestamp = DateTime.UtcNow }, cancellationToken);
    }
}

public class TeamMemberRemovedEventHandler : INotificationHandler<TeamMemberRemovedEvent>
{
    private readonly ILogger<TeamMemberRemovedEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public TeamMemberRemovedEventHandler(ILogger<TeamMemberRemovedEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(TeamMemberRemovedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Member removed - Team {TeamId}, Employee {EmployeeSysId}",
            notification.TeamId, notification.EmployeeSysId);
        await _publisher.PublishAsync("team.events", "team.member.removed",
            new { notification.TeamId, notification.EmployeeSysId, Action = "Removed", Timestamp = DateTime.UtcNow }, cancellationToken);
    }
}

public class TeamDeletedEventHandler : INotificationHandler<TeamDeletedEvent>
{
    private readonly ILogger<TeamDeletedEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public TeamDeletedEventHandler(ILogger<TeamDeletedEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(TeamDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Team deleted - {TeamId}", notification.TeamId);
        await _publisher.PublishAsync("team.events", "team.deleted",
            new { notification.TeamId, Timestamp = DateTime.UtcNow }, cancellationToken);
    }
}
