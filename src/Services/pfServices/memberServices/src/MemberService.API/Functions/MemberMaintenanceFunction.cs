using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace MemberService.API.Functions;

/// <summary>
/// Azure Timer Function — runs daily at 00:00 UTC to perform background member maintenance:
/// - Flags stale records
/// - Publishes daily summary events
/// </summary>
public class MemberMaintenanceFunction
{
    private readonly ILogger<MemberMaintenanceFunction> _logger;

    public MemberMaintenanceFunction(ILogger<MemberMaintenanceFunction> logger) => _logger = logger;

    [Function("MemberDailyMaintenance")]
    public async Task RunAsync([TimerTrigger("0 0 0 * * *")] TimerInfo timer)
    {
        _logger.LogInformation("MemberDailyMaintenance started at {Time}", DateTime.UtcNow);

        // TODO: inject IMemberRepository and flag members past retirement age, etc.
        await Task.Delay(100); // placeholder for async work

        _logger.LogInformation("MemberDailyMaintenance completed.");
    }
}
