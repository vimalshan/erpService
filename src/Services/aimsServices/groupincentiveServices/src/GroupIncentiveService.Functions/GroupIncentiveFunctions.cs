using GroupIncentiveService.Infrastructure.Persistence;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GroupIncentiveService.Functions;

/// <summary>
/// Timer-triggered Azure Function that runs every day at midnight
/// to send reminder notifications for pending incentive approvals.
/// </summary>
public class PendingIncentiveReminderFunction
{
    private readonly GroupIncentiveDbContext _dbContext;
    private readonly ILogger<PendingIncentiveReminderFunction> _logger;

    public PendingIncentiveReminderFunction(GroupIncentiveDbContext dbContext,
        ILogger<PendingIncentiveReminderFunction> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [Function("PendingIncentiveReminder")]
    public async Task Run([TimerTrigger("0 0 * * *")] TimerInfo timer, CancellationToken ct)
    {
        _logger.LogInformation("PendingIncentiveReminder triggered at {Time}", DateTime.UtcNow);

        var pendingCount = await _dbContext.GroupIncentiveMains
            .CountAsync(m => m.GrpIncAppStatus == "P", ct);

        _logger.LogInformation("Found {Count} pending incentive records requiring approval.", pendingCount);

        // TODO: Integrate with notification service (email/push) to alert approvers.
        // This is a placeholder showing the background task pattern.
    }
}

/// <summary>
/// Timer-triggered function that runs on the 1st of each month
/// to generate an incentive summary report.
/// </summary>
public class MonthlyIncentiveSummaryFunction
{
    private readonly GroupIncentiveDbContext _dbContext;
    private readonly ILogger<MonthlyIncentiveSummaryFunction> _logger;

    public MonthlyIncentiveSummaryFunction(GroupIncentiveDbContext dbContext,
        ILogger<MonthlyIncentiveSummaryFunction> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [Function("MonthlyIncentiveSummary")]
    public async Task Run([TimerTrigger("0 0 1 * *")] TimerInfo timer, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var prevMonth = now.Month == 1 ? 12 : now.Month - 1;
        var prevYear = now.Month == 1 ? now.Year - 1 : now.Year;

        _logger.LogInformation("Generating monthly summary for {Month}/{Year}", prevMonth, prevYear);

        var summary = await _dbContext.GroupIncentiveMains
            .Where(m => m.GrpIncIncMonth == prevMonth && m.GrpIncIncYear == prevYear)
            .GroupBy(m => m.GrpIncAppStatus)
            .Select(g => new { Status = g.Key, Count = g.Count(), Total = g.Sum(m => m.GrpIncTotalAmount) })
            .ToListAsync(ct);

        foreach (var s in summary)
            _logger.LogInformation("Status={Status}, Count={Count}, Total={Total:C}", s.Status, s.Count, s.Total);

        // TODO: Store summary to blob storage or emit event.
    }
}
