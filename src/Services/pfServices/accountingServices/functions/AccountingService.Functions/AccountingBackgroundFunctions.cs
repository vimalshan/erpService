using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AccountingService.Functions;

/// <summary>
/// Background Azure Function – runs nightly to reconcile GL balances
/// and archive old transaction records.
/// </summary>
public class AccountingBackgroundFunctions
{
    private readonly ILogger<AccountingBackgroundFunctions> _logger;

    public AccountingBackgroundFunctions(ILogger<AccountingBackgroundFunctions> logger)
        => _logger = logger;

    /// <summary>
    /// Nightly GL Reconciliation – runs at 02:00 UTC every day.
    /// CRON: "0 0 2 * * *" = sec min hour day month weekday
    /// </summary>
    [Function("NightlyGlReconciliation")]
    public void NightlyGlReconciliation(
        [TimerTrigger("0 0 2 * * *")] TimerInfo timer)
    {
        _logger.LogInformation("Nightly GL Reconciliation started at {Time}", DateTime.UtcNow);

        // TODO: Inject IMediator or direct db access to perform reconciliation
        // e.g. compare sum of debit vs credit per account, flag discrepancies

        _logger.LogInformation("Nightly GL Reconciliation completed at {Time}", DateTime.UtcNow);
    }

    /// <summary>
    /// Monthly trial-balance snapshot – runs on the 1st of every month at 01:00 UTC.
    /// </summary>
    [Function("MonthlyTrialBalanceSnapshot")]
    public void MonthlyTrialBalanceSnapshot(
        [TimerTrigger("0 0 1 1 * *")] TimerInfo timer)
    {
        _logger.LogInformation("Monthly Trial Balance Snapshot started at {Time}", DateTime.UtcNow);

        // TODO: Query vw_GLTrialBalance and store snapshot in Blob Storage
        _logger.LogInformation("Monthly Trial Balance Snapshot completed at {Time}", DateTime.UtcNow);
    }

    /// <summary>
    /// Weekly archive of cancelled transactions older than 90 days.
    /// Runs every Sunday at 03:00 UTC.
    /// </summary>
    [Function("WeeklyTransactionArchive")]
    public void WeeklyTransactionArchive(
        [TimerTrigger("0 0 3 * * 0")] TimerInfo timer)
    {
        _logger.LogInformation("Weekly Transaction Archive started at {Time}", DateTime.UtcNow);

        // TODO: Move old cancelled transactions to archive table / blob storage
        _logger.LogInformation("Weekly Transaction Archive completed at {Time}", DateTime.UtcNow);
    }
}
