using CanteenUnit.Infrastructure.Dapper;
using Microsoft.Extensions.Logging;

namespace CanteenUnit.Functions.Functions;

/// <summary>
/// Runs daily to sync stale canteen data / cleanup expired access records.
/// </summary>
public class CanteenDataSyncFunction
{
    private readonly CanteenUnitDapperRepository _dapper;
    private readonly ILogger<CanteenDataSyncFunction> _logger;

    public CanteenDataSyncFunction(CanteenUnitDapperRepository dapper, ILogger<CanteenDataSyncFunction> logger)
    {
        _dapper = dapper;
        _logger = logger;
    }

    // Runs every day at 02:00 UTC
    // Trigger: "0 0 2 * * *" (CRON)
    public async Task RunAsync(CancellationToken ct)
    {
        _logger.LogInformation("CanteenDataSyncFunction executing at {Time}", DateTime.UtcNow);

        var units = await _dapper.GetUnitsWithAccessCountAsync();
        int count = 0;
        foreach (var unit in units)
        {
            count++;
            _logger.LogInformation("Unit {Code} — {Name} has {Count} active accesses",
                (object)unit.UN_COM_COD, (object)unit.UN_UNT_NAME, (object)unit.AccessCount);
        }

        _logger.LogInformation("CanteenDataSyncFunction finished. Processed {Count} units.", count);
    }
}
