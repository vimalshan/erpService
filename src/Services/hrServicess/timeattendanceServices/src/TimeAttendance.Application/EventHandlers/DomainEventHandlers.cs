using MediatR;
using Microsoft.Extensions.Logging;
using TimeAttendance.Domain.Events;

namespace TimeAttendance.Application.EventHandlers;

public class AbsenteeismDetailCreatedEventHandler(ILogger<AbsenteeismDetailCreatedEventHandler> logger)
    : INotificationHandler<AbsenteeismDetailCreatedEvent>
{
    public Task Handle(AbsenteeismDetailCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Domain Event: AbsenteeismDetail created - ID: {Id}, Unit: {UnitId}, Period: {Year}-{Month:D2}",
            notification.AbsenteeismId, notification.UnitId, notification.Year, notification.Month);
        return Task.CompletedTask;
    }
}

public class AbsenteeismDetailUpdatedEventHandler(ILogger<AbsenteeismDetailUpdatedEventHandler> logger)
    : INotificationHandler<AbsenteeismDetailUpdatedEvent>
{
    public Task Handle(AbsenteeismDetailUpdatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Domain Event: AbsenteeismDetail updated - ID: {Id}, Unit: {UnitId}",
            notification.AbsenteeismId, notification.UnitId);
        return Task.CompletedTask;
    }
}

public class AbsenteeismMisCreatedEventHandler(ILogger<AbsenteeismMisCreatedEventHandler> logger)
    : INotificationHandler<AbsenteeismMisCreatedEvent>
{
    public Task Handle(AbsenteeismMisCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Domain Event: AbsenteeismMIS created - ID: {Id}, Unit: {UnitId}, Month: {Month}",
            notification.MisId, notification.UnitId, notification.Month);
        return Task.CompletedTask;
    }
}
