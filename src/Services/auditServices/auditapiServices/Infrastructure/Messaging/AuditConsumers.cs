using AuditService.Domain.Events;
using MassTransit;

namespace AuditService.Infrastructure.Messaging;

public class AuditCreatedConsumer : IConsumer<AuditCreatedEvent>
{
    private readonly ILogger<AuditCreatedConsumer> _logger;
    public AuditCreatedConsumer(ILogger<AuditCreatedConsumer> logger) => _logger = logger;
    public Task Consume(ConsumeContext<AuditCreatedEvent> context)
    {
        _logger.LogInformation("Audit created: {AuditId}", context.Message.AuditId);
        return Task.CompletedTask;
    }
}

public class AuditStatusChangedConsumer : IConsumer<AuditStatusChangedEvent>
{
    private readonly ILogger<AuditStatusChangedConsumer> _logger;
    public AuditStatusChangedConsumer(ILogger<AuditStatusChangedConsumer> logger) => _logger = logger;
    public Task Consume(ConsumeContext<AuditStatusChangedEvent> context)
    {
        _logger.LogInformation("Audit {AuditId} status changed from {Old} to {New}",
            context.Message.AuditId, context.Message.OldStatus, context.Message.NewStatus);
        return Task.CompletedTask;
    }
}
