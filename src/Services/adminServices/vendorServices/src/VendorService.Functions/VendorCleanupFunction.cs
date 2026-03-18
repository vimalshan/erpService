using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.Timer;
using Microsoft.Extensions.Logging;

namespace VendorService.Functions;

/// <summary>
/// Cleans up inactive vendors and stale data on a weekly schedule.
/// </summary>
public sealed class VendorCleanupFunction
{
    private readonly ILogger<VendorCleanupFunction> _logger;

    public VendorCleanupFunction(ILogger<VendorCleanupFunction> logger)
    {
        _logger = logger;
    }

    // Runs every Sunday at midnight UTC
    [Function(nameof(VendorCleanupFunction))]
    public async Task RunAsync(
        [TimerTrigger("0 0 0 * * 0")] TimerInfo timer)
    {
        _logger.LogInformation(
            "Vendor Cleanup started at {Time}. IsPastDue: {IsPastDue}",
            DateTime.UtcNow,
            timer.IsPastDue);

        // TODO: Archive or remove vendors marked inactive for > 1 year
        // TODO: Clean up orphaned TDS vendor records
        // TODO: Purge old blob storage files

        _logger.LogInformation("Vendor Cleanup completed at {Time}", DateTime.UtcNow);
        await Task.CompletedTask;
    }
}
