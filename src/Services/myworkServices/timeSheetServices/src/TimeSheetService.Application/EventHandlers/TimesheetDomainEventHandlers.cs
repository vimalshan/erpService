using MediatR;
using Microsoft.Extensions.Logging;
using TimeSheetService.Application.Interfaces;
using TimeSheetService.Domain.Events;

namespace TimeSheetService.Application.EventHandlers;

public class TimesheetSubmittedEventHandler : INotificationHandler<TimesheetSubmittedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<TimesheetSubmittedEventHandler> _logger;

    public TimesheetSubmittedEventHandler(IMessagePublisher publisher, ILogger<TimesheetSubmittedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(TimesheetSubmittedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timesheet submitted: TimeId={TimeId}, EmployeeId={EmployeeId}",
            notification.TimeId, notification.EmployeeSysId);
        await _publisher.PublishAsync("timesheet.submitted", notification, cancellationToken);
    }
}

public class TimesheetUpdatedEventHandler : INotificationHandler<TimesheetUpdatedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<TimesheetUpdatedEventHandler> _logger;

    public TimesheetUpdatedEventHandler(IMessagePublisher publisher, ILogger<TimesheetUpdatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(TimesheetUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timesheet updated: TimeId={TimeId}", notification.TimeId);
        await _publisher.PublishAsync("timesheet.updated", notification, cancellationToken);
    }
}

public class TimesheetDeletedEventHandler : INotificationHandler<TimesheetDeletedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<TimesheetDeletedEventHandler> _logger;

    public TimesheetDeletedEventHandler(IMessagePublisher publisher, ILogger<TimesheetDeletedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(TimesheetDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Timesheet deleted: TimeId={TimeId}", notification.TimeId);
        await _publisher.PublishAsync("timesheet.deleted", notification, cancellationToken);
    }
}

public class TcTimesheetSubmittedEventHandler : INotificationHandler<TcTimesheetSubmittedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<TcTimesheetSubmittedEventHandler> _logger;

    public TcTimesheetSubmittedEventHandler(IMessagePublisher publisher, ILogger<TcTimesheetSubmittedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(TcTimesheetSubmittedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("TC Timesheet submitted: TimeId={TimeId}, EmployeeId={EmployeeId}",
            notification.TimeId, notification.EmployeeSysId);
        await _publisher.PublishAsync("tc.timesheet.submitted", notification, cancellationToken);
    }
}
