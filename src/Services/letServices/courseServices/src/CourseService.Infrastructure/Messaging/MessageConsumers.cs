using CourseService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CourseService.Infrastructure.Messaging;

/// <summary>
/// MediatR handlers that publish domain events to RabbitMQ.
/// </summary>
public class CourseCreatedMessageConsumer(IMessagePublisher publisher, ILogger<CourseCreatedMessageConsumer> logger)
    : INotificationHandler<CourseCreatedEvent>
{
    public async Task Handle(CourseCreatedEvent notification, CancellationToken ct)
    {
        await publisher.PublishAsync(new
        {
            notification.CourseId,
            notification.CourseDescription,
            notification.OccurredOn
        }, "course.created", ct);

        logger.LogInformation("CourseCreated event published to RabbitMQ for CourseId {CourseId}", notification.CourseId);
    }
}

public class ParticipantRegisteredMessageConsumer(IMessagePublisher publisher, ILogger<ParticipantRegisteredMessageConsumer> logger)
    : INotificationHandler<ParticipantRegisteredEvent>
{
    public async Task Handle(ParticipantRegisteredEvent notification, CancellationToken ct)
    {
        await publisher.PublishAsync(new
        {
            notification.CourseId,
            notification.UserCode,
            notification.EnrollmentDate,
            notification.OccurredOn
        }, "course.participant.registered", ct);

        logger.LogInformation("ParticipantRegistered event published to RabbitMQ for User {UserCode} on Course {CourseId}",
            notification.UserCode, notification.CourseId);
    }
}

public class ParticipantCancelledMessageConsumer(IMessagePublisher publisher, ILogger<ParticipantCancelledMessageConsumer> logger)
    : INotificationHandler<ParticipantCancelledEvent>
{
    public async Task Handle(ParticipantCancelledEvent notification, CancellationToken ct)
    {
        await publisher.PublishAsync(new
        {
            notification.CourseId,
            notification.UserCode,
            notification.CancellationDate,
            notification.OccurredOn
        }, "course.participant.cancelled", ct);

        logger.LogInformation("ParticipantCancelled event published to RabbitMQ for User {UserCode} on Course {CourseId}",
            notification.UserCode, notification.CourseId);
    }
}
