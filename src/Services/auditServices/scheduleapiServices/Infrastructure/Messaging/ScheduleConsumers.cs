using ScheduleService.Domain.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace ScheduleService.Infrastructure.Messaging;

public class AuditScheduledConsumer : IConsumer<AuditScheduledEvent>
{
    private readonly ILogger<AuditScheduledConsumer> _logger;
    public AuditScheduledConsumer(ILogger<AuditScheduledConsumer> logger) { _logger = logger; }
    public Task Consume(ConsumeContext<AuditScheduledEvent> context)
    {
        _logger.LogInformation("Audit scheduled: {AuditNumber} for site {SiteId} on {Date}", context.Message.AuditNumber, context.Message.SiteId, context.Message.ScheduledDate);
        return Task.CompletedTask;
    }
}

public class AuditCompletedConsumer : IConsumer<AuditCompletedEvent>
{
    private readonly ILogger<AuditCompletedConsumer> _logger;
    public AuditCompletedConsumer(ILogger<AuditCompletedConsumer> logger) { _logger = logger; }
    public Task Consume(ConsumeContext<AuditCompletedEvent> context)
    {
        _logger.LogInformation("Audit completed: {AuditNumber} on {Date}", context.Message.AuditNumber, context.Message.CompletedDate);
        return Task.CompletedTask;
    }
}
