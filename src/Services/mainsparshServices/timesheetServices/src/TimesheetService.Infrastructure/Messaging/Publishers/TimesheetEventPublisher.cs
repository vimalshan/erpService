using MassTransit;
using Microsoft.Extensions.Logging;
using TimesheetService.Domain.Events;

namespace TimesheetService.Infrastructure.Messaging.Publishers;

/// <summary>
/// Publishes domain events to RabbitMQ exchanges via MassTransit.
/// </summary>
public sealed class TimesheetEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<TimesheetEventPublisher> _logger;

    public TimesheetEventPublisher(IPublishEndpoint publishEndpoint, ILogger<TimesheetEventPublisher> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger          = logger;
    }

    public async Task PublishAsync(TimesheetSubmittedEvent domainEvent, CancellationToken ct = default)
    {
        _logger.LogInformation("Publishing TimesheetSubmitted for {TimesheetId}", domainEvent.TimesheetId);
        await _publishEndpoint.Publish(new TimesheetSubmittedIntegrationEvent
        {
            TimesheetId = domainEvent.TimesheetId,
            EmployeeId  = domainEvent.EmployeeId,
            OccurredOn  = domainEvent.OccurredOn
        }, ct);
    }

    public async Task PublishAsync(TimesheetApprovedEvent domainEvent, CancellationToken ct = default)
    {
        _logger.LogInformation("Publishing TimesheetApproved for {TimesheetId}", domainEvent.TimesheetId);
        await _publishEndpoint.Publish(new TimesheetApprovedIntegrationEvent
        {
            TimesheetId = domainEvent.TimesheetId,
            EmployeeId  = domainEvent.EmployeeId,
            ApproverId  = domainEvent.ApproverId,
            OccurredOn  = domainEvent.OccurredOn
        }, ct);
    }
}

// Integration event contracts (messages published to the bus)
public sealed class TimesheetSubmittedIntegrationEvent
{
    public long     TimesheetId { get; set; }
    public long     EmployeeId  { get; set; }
    public DateTime OccurredOn  { get; set; }
}

public sealed class TimesheetApprovedIntegrationEvent
{
    public long     TimesheetId { get; set; }
    public long     EmployeeId  { get; set; }
    public long     ApproverId  { get; set; }
    public DateTime OccurredOn  { get; set; }
}
