using MediatR;
using Microsoft.Extensions.Logging;
using TaskServices.Domain.Events;
using TaskServices.Infrastructure.Messaging;

namespace TaskServices.Infrastructure.Services;

public class TaskMailCreatedEventHandler : INotificationHandler<TaskMailCreatedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<TaskMailCreatedEventHandler> _logger;

    public TaskMailCreatedEventHandler(IMessagePublisher publisher, ILogger<TaskMailCreatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(TaskMailCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain event: TaskMailCreated - MID={MID}, SYSID={SYSID}", notification.MailId, notification.SystemUserId);

        await _publisher.PublishAsync("task-events", "task.mail.created", new
        {
            notification.MailId,
            notification.SystemUserId,
            OccurredAt = DateTime.UtcNow
        }, cancellationToken);
    }
}

public class TaskMailReassignedEventHandler : INotificationHandler<TaskMailReassignedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<TaskMailReassignedEventHandler> _logger;

    public TaskMailReassignedEventHandler(IMessagePublisher publisher, ILogger<TaskMailReassignedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(TaskMailReassignedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain event: TaskMailReassigned - MID={MID}, Old={Old}, New={New}",
            notification.MailId, notification.OldSystemUserId, notification.NewSystemUserId);

        await _publisher.PublishAsync("task-events", "task.mail.reassigned", new
        {
            notification.MailId,
            notification.OldSystemUserId,
            notification.NewSystemUserId,
            OccurredAt = DateTime.UtcNow
        }, cancellationToken);
    }
}
