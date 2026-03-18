using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace UserSecurityService.Functions;

/// <summary>
/// Timer-triggered Azure Function that runs daily to deactivate expired user sessions
/// and clean up accounts with a CloseDate in the past.
/// Cron: every day at 02:00 UTC.
/// </summary>
public class CleanupExpiredUsersFunction(ILogger<CleanupExpiredUsersFunction> logger)
{
    [Function("CleanupExpiredUsers")]
    public Task Run([TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo)
    {
        logger.LogInformation("CleanupExpiredUsers triggered at {Time}", DateTime.UtcNow);
        // In a real implementation inject IUserProfileRepository and query/deactivate expired users.
        logger.LogInformation("Finished CleanupExpiredUsers at {Time}", DateTime.UtcNow);
        return Task.CompletedTask;
    }
}

/// <summary>
/// Timer-triggered Azure Function that reports audit log statistics daily.
/// Cron: every day at 03:00 UTC.
/// </summary>
public class AuditReportFunction(ILogger<AuditReportFunction> logger)
{
    [Function("DailyAuditReport")]
    public Task Run([TimerTrigger("0 0 3 * * *")] TimerInfo timerInfo)
    {
        logger.LogInformation("DailyAuditReport triggered at {Time}", DateTime.UtcNow);
        // Query USER_UNITMAPLOG, USER_CALENDERMAP_LOG, USER_MENUMAP_LOG and send report.
        logger.LogInformation("DailyAuditReport completed at {Time}", DateTime.UtcNow);
        return Task.CompletedTask;
    }
}

/// <summary>
/// Timer-triggered function that purges old password change records beyond a retention period.
/// Cron: every Sunday at 04:00 UTC.
/// </summary>
public class PasswordChangeRetentionFunction(ILogger<PasswordChangeRetentionFunction> logger)
{
    private static readonly TimeSpan RetentionPeriod = TimeSpan.FromDays(365);

    [Function("PasswordChangeRetention")]
    public Task Run([TimerTrigger("0 0 4 * * 0")] TimerInfo timerInfo)
    {
        var cutoff = DateTime.UtcNow - RetentionPeriod;
        logger.LogInformation("PasswordChangeRetention: purging records before {Cutoff}", cutoff);
        // Inject repository and delete EmpPasswordChange records older than cutoff.
        return Task.CompletedTask;
    }
}
