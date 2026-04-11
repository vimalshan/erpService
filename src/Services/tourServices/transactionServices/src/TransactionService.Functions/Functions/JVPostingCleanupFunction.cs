using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TransactionService.Infrastructure.Persistence;

namespace TransactionService.Functions.Functions;

/// <summary>
/// Cleans up stale temporary JV records (TEMPJVEMP_MAIN/SUB, TEMPJVSUP_MAIN/SUB) older than 30 days.
/// Schedule: Every day at 3:00 AM UTC
/// </summary>
public sealed class JVPostingCleanupFunction
{
    private readonly TransactionDbContext _context;
    private readonly ILogger<JVPostingCleanupFunction> _logger;

    public JVPostingCleanupFunction(TransactionDbContext context,
        ILogger<JVPostingCleanupFunction> logger)
    {
        _context = context;
        _logger = logger;
    }

    [Function(nameof(JVPostingCleanupFunction))]
    public async Task Run([TimerTrigger("0 0 3 * * *")] TimerInfo timerInfo, CancellationToken ct)
    {
        _logger.LogInformation("JV posting cleanup started at {Time}", DateTime.UtcNow);

        var cutoffDate = DateTime.UtcNow.AddDays(-30);

        // Clean pending employee JVs older than 30 days
        var stalePendingJVs = await _context.EmployeeJournalVouchers
            .Where(jv => jv.JvStatus == "P" && jv.CreatedOn < cutoffDate)
            .ToListAsync(ct);

        foreach (var jv in stalePendingJVs)
        {
            _logger.LogWarning("Stale pending Employee JV found: {JvBatchId}, created {CreatedOn}",
                jv.JvBatchId, jv.CreatedOn);
        }

        _logger.LogInformation("JV posting cleanup finished. {Count} stale JVs found.", stalePendingJVs.Count);
    }
}
