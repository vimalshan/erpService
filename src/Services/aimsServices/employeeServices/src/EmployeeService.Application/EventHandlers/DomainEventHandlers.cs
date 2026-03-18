using MediatR;
using Microsoft.Extensions.Logging;
using EmployeeService.Domain.Events;
using EmployeeService.Application.Interfaces;

namespace EmployeeService.Application.EventHandlers;

/// <summary>Publishes ApproverAssignedEvent to messaging bus after the fact.</summary>
public sealed class ApproverAssignedEventHandler : INotificationHandler<ApproverAssignedEvent>
{
    private readonly IEventPublisher _publisher;
    private readonly ILogger<ApproverAssignedEventHandler> _logger;

    public ApproverAssignedEventHandler(IEventPublisher publisher, ILogger<ApproverAssignedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(ApproverAssignedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain event: ApproverAssigned for employee {EmpSysId} at level {Level}",
            notification.EmpSysId, notification.Level);
        await _publisher.PublishAsync(notification, "approver.assigned", cancellationToken);
    }
}

/// <summary>Handles CalendarMappedEvent domain event.</summary>
public sealed class CalendarMappedEventHandler : INotificationHandler<CalendarMappedEvent>
{
    private readonly IEventPublisher _publisher;
    private readonly ILogger<CalendarMappedEventHandler> _logger;

    public CalendarMappedEventHandler(IEventPublisher publisher, ILogger<CalendarMappedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(CalendarMappedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain event: CalendarMapped for employee {EmpSysId}", notification.EmpSysId);
        await _publisher.PublishAsync(notification, "calendar.mapped", cancellationToken);
    }
}

/// <summary>Handles TimeInfoUpdatedEvent domain event.</summary>
public sealed class TimeInfoUpdatedEventHandler : INotificationHandler<TimeInfoUpdatedEvent>
{
    private readonly ILogger<TimeInfoUpdatedEventHandler> _logger;

    public TimeInfoUpdatedEventHandler(ILogger<TimeInfoUpdatedEventHandler> logger) => _logger = logger;

    public Task Handle(TimeInfoUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain event: TimeInfoUpdated — Employee {EmpSysId}, Flag {Flag}",
            notification.EmpSysId, notification.AttFlag);
        return Task.CompletedTask;
    }
}
