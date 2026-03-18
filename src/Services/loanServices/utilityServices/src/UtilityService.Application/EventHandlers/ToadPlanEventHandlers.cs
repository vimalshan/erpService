using MediatR;
using Microsoft.Extensions.Logging;
using UtilityService.Domain.Events;

namespace UtilityService.Application.EventHandlers;

public class ToadPlanCreatedEventHandler : INotificationHandler<ToadPlanCreatedEvent>
{
    private readonly ILogger<ToadPlanCreatedEventHandler> _logger;

    public ToadPlanCreatedEventHandler(ILogger<ToadPlanCreatedEventHandler> logger)
        => _logger = logger;

    public Task Handle(ToadPlanCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "ToadPlanSql created - StatementId: {StatementId}, User: {Username}, EventId: {EventId}",
            notification.StatementId, notification.Username, notification.EventId);
        return Task.CompletedTask;
    }
}

public class ToadPlanUpdatedEventHandler : INotificationHandler<ToadPlanUpdatedEvent>
{
    private readonly ILogger<ToadPlanUpdatedEventHandler> _logger;

    public ToadPlanUpdatedEventHandler(ILogger<ToadPlanUpdatedEventHandler> logger)
        => _logger = logger;

    public Task Handle(ToadPlanUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "ToadPlanSql updated - StatementId: {StatementId}, User: {Username}, EventId: {EventId}",
            notification.StatementId, notification.Username, notification.EventId);
        return Task.CompletedTask;
    }
}

public class ToadPlanDeletedEventHandler : INotificationHandler<ToadPlanDeletedEvent>
{
    private readonly ILogger<ToadPlanDeletedEventHandler> _logger;

    public ToadPlanDeletedEventHandler(ILogger<ToadPlanDeletedEventHandler> logger)
        => _logger = logger;

    public Task Handle(ToadPlanDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "ToadPlanSql deleted - StatementId: {StatementId}, EventId: {EventId}",
            notification.StatementId, notification.EventId);
        return Task.CompletedTask;
    }
}
