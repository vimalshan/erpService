using AuditLogService.Domain.Events;
using AuditLogService.Infrastructure.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditLogService.Application.EventHandlers;

public class AuditLogCreatedEventHandler : INotificationHandler<AuditLogCreatedEvent>
{
    private readonly RabbitMqPublisher _publisher;
    private readonly ILogger<AuditLogCreatedEventHandler> _logger;

    public AuditLogCreatedEventHandler(RabbitMqPublisher publisher, ILogger<AuditLogCreatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(AuditLogCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain event: AuditLog created for {Table} record {RecordId}",
            notification.AuditLog.TableName,
            notification.AuditLog.RecordId);

        try
        {
            await _publisher.PublishAsync(new
            {
                EventType = "AuditLogCreated",
                notification.AuditLog.TableName,
                notification.AuditLog.RecordId,
                Action = notification.AuditLog.Action.Value,
                notification.AuditLog.ChangedBy,
                notification.AuditLog.ChangeDate
            }, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to publish AuditLogCreated event to RabbitMQ. Will retry later.");
        }
    }
}
