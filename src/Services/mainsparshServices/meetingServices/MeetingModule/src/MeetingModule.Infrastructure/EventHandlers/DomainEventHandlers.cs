using MediatR;
using MeetingModule.Domain.Events;
using MeetingModule.Infrastructure.Messaging;
using Microsoft.Extensions.Logging;

namespace MeetingModule.Infrastructure.EventHandlers;

public class MeetingCreatedEventHandler(IMessagePublisher publisher, ILogger<MeetingCreatedEventHandler> logger)
    : INotificationHandler<MeetingCreatedEvent>
{
    public async Task Handle(MeetingCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain event: Meeting created - {Title} (ID: {Id})",
            notification.Meeting.MeetingTitle, notification.Meeting.MeetingId);

        await publisher.PublishAsync("meeting.created", new
        {
            notification.Meeting.MeetingId,
            notification.Meeting.MeetingTitle,
            notification.Meeting.MeetingDate,
            notification.Meeting.OrganizerId,
            Timestamp = DateTime.UtcNow
        }, ct);
    }
}

public class MeetingStatusChangedEventHandler(IMessagePublisher publisher, ILogger<MeetingStatusChangedEventHandler> logger)
    : INotificationHandler<MeetingStatusChangedEvent>
{
    public async Task Handle(MeetingStatusChangedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Domain event: Meeting status changed - {Title} -> {Status}",
            notification.Meeting.MeetingTitle, notification.NewStatus);

        await publisher.PublishAsync($"meeting.status.{notification.NewStatus.ToLowerInvariant()}", new
        {
            notification.Meeting.MeetingId,
            notification.Meeting.MeetingTitle,
            notification.NewStatus,
            Timestamp = DateTime.UtcNow
        }, ct);
    }
}
