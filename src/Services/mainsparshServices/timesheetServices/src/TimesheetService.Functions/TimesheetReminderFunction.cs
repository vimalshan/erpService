using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TimesheetService.Domain.Interfaces;

namespace TimesheetService.Functions;

/// <summary>
/// Azure Function: runs on a timer schedule to remind employees with DRAFT timesheets
/// that have not been submitted within the past 7 days.
/// Schedule: every day at 08:00 UTC (cron: 0 0 8 * * *)
/// </summary>
public sealed class TimesheetReminderFunction
{
    private readonly ITimesheetRepository _repository;
    private readonly ILogger<TimesheetReminderFunction> _logger;

    public TimesheetReminderFunction(ITimesheetRepository repository, ILogger<TimesheetReminderFunction> logger)
    {
        _repository = repository;
        _logger     = logger;
    }

    [Function(nameof(TimesheetReminderFunction))]
    public async Task Run(
        [TimerTrigger("0 0 8 * * *")] TimerInfo timerInfo,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("TimesheetReminderFunction triggered at {UtcNow}", DateTime.UtcNow);

        var cutOff   = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-7));
        var pending  = await _repository.GetPendingTimesheetsAsync(cancellationToken);

        var overdue = pending.Where(t => t.WorkDate <= cutOff).ToList();

        if (overdue.Count == 0)
        {
            _logger.LogInformation("No overdue timesheets found.");
            return;
        }

        _logger.LogWarning("{Count} overdue timesheets found. Sending reminders...", overdue.Count);

        foreach (var t in overdue)
        {
            // In production, dispatch a notification command or email here.
            _logger.LogInformation(
                "Reminder: Timesheet {TimesheetId} for Employee {EmployeeId} (WorkDate: {WorkDate}) is pending approval.",
                t.TimesheetId, t.EmployeeId, t.WorkDate);
        }
    }
}
