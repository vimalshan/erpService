using CourseService.Domain.Common;
using CourseService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CourseService.Application.EventHandlers;

public class CourseCreatedEventHandler(ILogger<CourseCreatedEventHandler> logger)
    : INotificationHandler<CourseCreatedEvent>
{
    public Task Handle(CourseCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: CourseCreated - CourseId: {CourseId}, Description: {Description}",
            notification.CourseId, notification.CourseDescription);
        return Task.CompletedTask;
    }
}

public class ParticipantRegisteredEventHandler(ILogger<ParticipantRegisteredEventHandler> logger)
    : INotificationHandler<ParticipantRegisteredEvent>
{
    public Task Handle(ParticipantRegisteredEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: ParticipantRegistered - CourseId: {CourseId}, User: {UserCode}",
            notification.CourseId, notification.UserCode);
        return Task.CompletedTask;
    }
}

public class ParticipantCancelledEventHandler(ILogger<ParticipantCancelledEventHandler> logger)
    : INotificationHandler<ParticipantCancelledEvent>
{
    public Task Handle(ParticipantCancelledEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: ParticipantCancelled - CourseId: {CourseId}, User: {UserCode}",
            notification.CourseId, notification.UserCode);
        return Task.CompletedTask;
    }
}

public class AttendanceUpdatedEventHandler(ILogger<AttendanceUpdatedEventHandler> logger)
    : INotificationHandler<AttendanceUpdatedEvent>
{
    public Task Handle(AttendanceUpdatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain Event: AttendanceUpdated - CourseId: {CourseId}, User: {UserCode}, Status: {Status}",
            notification.CourseId, notification.UserCode, notification.AttendanceStatus);
        return Task.CompletedTask;
    }
}
