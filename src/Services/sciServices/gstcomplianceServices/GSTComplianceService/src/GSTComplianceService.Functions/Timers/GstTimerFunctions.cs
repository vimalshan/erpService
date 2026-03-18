using GSTComplianceService.Infrastructure.Dapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace GSTComplianceService.Functions.Timers;

/// <summary>
/// Timer-triggered function that runs every day at midnight (UTC)
/// to archive inactive GST registrations.
/// </summary>
public class GstArchiveTimerFunction
{
    private readonly IGstDapperRepository _dapperRepo;
    private readonly ILogger<GstArchiveTimerFunction> _logger;

    public GstArchiveTimerFunction(IGstDapperRepository dapperRepo, ILogger<GstArchiveTimerFunction> logger)
    {
        _dapperRepo = dapperRepo;
        _logger = logger;
    }

    [Function(nameof(GstArchiveTimerFunction))]
    public async Task Run(
        [TimerTrigger("0 0 0 * * *")] TimerInfo timerInfo,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("GST Archive Timer triggered at {Time}", DateTime.UtcNow);

        // TODO: Query inactive registrations older than retention period and archive them
        // Example: await _archiveService.ArchiveInactiveRegistrationsAsync(cancellationToken);

        if (timerInfo.ScheduleStatus is { } status)
            _logger.LogInformation("Next trigger scheduled at {NextRun}", status.Next);
    }
}

/// <summary>
/// Timer-triggered function that runs every hour to sync GST data from Oracle.
/// </summary>
public class GstOracleSyncTimerFunction
{
    private readonly ILogger<GstOracleSyncTimerFunction> _logger;

    public GstOracleSyncTimerFunction(ILogger<GstOracleSyncTimerFunction> logger)
    {
        _logger = logger;
    }

    [Function(nameof(GstOracleSyncTimerFunction))]
    public Task Run(
        [TimerTrigger("0 0 * * * *")] TimerInfo timerInfo,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("GST Oracle Sync Timer triggered at {Time}", DateTime.UtcNow);
        // TODO: Pull latest vendor/customer data from Oracle EBS and update local cache
        return Task.CompletedTask;
    }
}
