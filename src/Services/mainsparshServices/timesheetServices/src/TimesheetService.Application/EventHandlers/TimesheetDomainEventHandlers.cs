using MediatR;
using Microsoft.Extensions.Logging;
using TimesheetService.Domain.Events;

namespace TimesheetService.Application.EventHandlers;

public sealed class TimesheetCreatedEventHandler : INotificationHandler<TimesheetCreatedEvent>
{
    private readonly ILogger<TimesheetCreatedEventHandler> _logger;
    public TimesheetCreatedEventHandler(ILogger<TimesheetCreatedEventHandler> logger) => _logger = logger;

    public Task Handle(TimesheetCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Timesheet {TimesheetId} created for Employee {EmployeeId}",
            notification.TimesheetId, notification.EmployeeId);
        return Task.CompletedTask;
    }
}

public sealed class TimesheetSubmittedEventHandler : INotificationHandler<TimesheetSubmittedEvent>
{
    private readonly ILogger<TimesheetSubmittedEventHandler> _logger;
    public TimesheetSubmittedEventHandler(ILogger<TimesheetSubmittedEventHandler> logger) => _logger = logger;

    public Task Handle(TimesheetSubmittedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Timesheet {TimesheetId} submitted for Employee {EmployeeId}",
            notification.TimesheetId, notification.EmployeeId);
        return Task.CompletedTask;
    }
}

public sealed class TimesheetApprovedEventHandler : INotificationHandler<TimesheetApprovedEvent>
{
    private readonly ILogger<TimesheetApprovedEventHandler> _logger;
    public TimesheetApprovedEventHandler(ILogger<TimesheetApprovedEventHandler> logger) => _logger = logger;

    public Task Handle(TimesheetApprovedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Timesheet {TimesheetId} approved by {ApproverId}",
            notification.TimesheetId, notification.ApproverId);
        return Task.CompletedTask;
    }
}

public sealed class TimesheetRejectedEventHandler : INotificationHandler<TimesheetRejectedEvent>
{
    private readonly ILogger<TimesheetRejectedEventHandler> _logger;
    public TimesheetRejectedEventHandler(ILogger<TimesheetRejectedEventHandler> logger) => _logger = logger;

    public Task Handle(TimesheetRejectedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Timesheet {TimesheetId} rejected. Reason: {Reason}",
            notification.TimesheetId, notification.RejectionReason);
        return Task.CompletedTask;
    }
}
