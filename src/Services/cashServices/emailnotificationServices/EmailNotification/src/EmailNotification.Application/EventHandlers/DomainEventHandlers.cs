using MediatR;
using Microsoft.Extensions.Logging;

namespace EmailNotification.Application.EventHandlers;

/// <summary>
/// Handler for EmailTypeCreatedEvent - fires when a new email type is created
/// </summary>
public class EmailTypeCreatedEventHandler : INotificationHandler<Domain.Events.EmailTypeCreatedEvent>
{
    private readonly ILogger<EmailTypeCreatedEventHandler> _logger;

    public EmailTypeCreatedEventHandler(ILogger<EmailTypeCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(Domain.Events.EmailTypeCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "EmailTypeCreatedEvent triggered - EmailTypeId: {EmailTypeId}, Name: {EmailName}, CreatedAt: {CreatedAt}",
            domainEvent.AggregateId,
            domainEvent.EmailName,
            domainEvent.OccurredAt);

        // TODO: Implement event-driven actions:
        // 1. Publish message to RabbitMQ for async processing
        // 2. Send notification email to admins
        // 3. Log audit trail to event store
        // 4. Trigger webhooks to external systems

        await Task.CompletedTask;
    }
}

/// <summary>
/// Handler for EmailTypeUpdatedEvent - fires when an email type is updated
/// </summary>
public class EmailTypeUpdatedEventHandler : INotificationHandler<Domain.Events.EmailTypeUpdatedEvent>
{
    private readonly ILogger<EmailTypeUpdatedEventHandler> _logger;

    public EmailTypeUpdatedEventHandler(ILogger<EmailTypeUpdatedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(Domain.Events.EmailTypeUpdatedEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "EmailTypeUpdatedEvent triggered - EmailTypeId: {EmailTypeId}, Name: {EmailName}, UpdatedAt: {UpdatedAt}",
            domainEvent.AggregateId,
            domainEvent.EmailName,
            domainEvent.OccurredAt);

        // TODO: Implement event-driven actions:
        // 1. Notify affected subscribers about configuration change
        // 2. Update cache/in-memory storage
        // 3. Log audit trail
        // 4. Trigger validation of updated configuration

        await Task.CompletedTask;
    }
}

/// <summary>
/// Handler for RecipientAddedEvent - fires when a recipient is added to an email type
/// </summary>
public class RecipientAddedEventHandler : INotificationHandler<Domain.Events.RecipientAddedEvent>
{
    private readonly ILogger<RecipientAddedEventHandler> _logger;

    public RecipientAddedEventHandler(ILogger<RecipientAddedEventHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(Domain.Events.RecipientAddedEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "RecipientAddedEvent triggered - EmailTypeId: {EmailTypeId}, Email: {Email}, AddedAt: {AddedAt}",
            domainEvent.AggregateId,
            domainEvent.EmailAddress,
            domainEvent.OccurredAt);

        // TODO: Implement event-driven actions:
        // 1. Send confirmation email to new recipient
        // 2. Add to mail service distribution list
        // 3. Log audit trail
        // 4. Update recipient database indexes

        await Task.CompletedTask;
    }
}
