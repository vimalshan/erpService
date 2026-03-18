using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TimesheetService.Domain.Interfaces;

namespace TimesheetService.Functions;

/// <summary>
/// Azure Function: generates monthly summary reports for approved timesheets.
/// Schedule: 1st of every month at 06:00 UTC (cron: 0 0 6 1 * *)
/// </summary>
public sealed class MonthlyTimesheetReportFunction
{
    private readonly ITimesheetRepository _repository;
    private readonly ILogger<MonthlyTimesheetReportFunction> _logger;

    public MonthlyTimesheetReportFunction(ITimesheetRepository repository, ILogger<MonthlyTimesheetReportFunction> logger)
    {
        _repository = repository;
        _logger     = logger;
    }

    [Function(nameof(MonthlyTimesheetReportFunction))]
    public async Task Run(
        [TimerTrigger("0 0 6 1 * *")] TimerInfo timerInfo,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        // Report covers the previous month
        var reportMonth = new DateOnly(now.Year, now.Month, 1).AddMonths(-1);

        _logger.LogInformation("Generating monthly report for {Year}-{Month}", reportMonth.Year, reportMonth.Month);

        var from = reportMonth;
        var to   = reportMonth.AddMonths(1).AddDays(-1);

        var timesheets = await _repository.GetAllAsync(1, int.MaxValue, cancellationToken);
        var approved   = timesheets
            .Where(t => t.WorkDate >= from && t.WorkDate <= to)
            .Where(t => t.ApprovalStatus.Value == "APPROVED")
            .GroupBy(t => t.EmployeeId)
            .Select(g => new { EmployeeId = g.Key, TotalHours = g.Sum(t => t.TotalHours ?? 0) })
            .ToList();

        foreach (var item in approved)
        {
            _logger.LogInformation(
                "Employee {EmployeeId}: {TotalHours}h approved for {Year}-{Month}",
                item.EmployeeId, item.TotalHours, reportMonth.Year, reportMonth.Month);
        }

        _logger.LogInformation("Monthly report generated. {Count} employees processed.", approved.Count);
    }
}
