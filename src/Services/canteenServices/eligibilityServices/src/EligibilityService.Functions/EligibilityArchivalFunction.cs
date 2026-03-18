using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace EligibilityService.Functions;

/// <summary>
/// Timer-triggered function that runs nightly to archive expired
/// CANTEEN_DAYWISE_ELIGIBILITY records into a historical partition.
/// </summary>
public class EligibilityArchivalFunction
{
    private readonly ILogger<EligibilityArchivalFunction> _logger;
    private readonly string _connectionString;

    public EligibilityArchivalFunction(
        ILogger<EligibilityArchivalFunction> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _connectionString = configuration["SqlConnectionString"]
            ?? throw new InvalidOperationException("SqlConnectionString is not configured.");
    }

    // Runs every night at 02:00 UTC
    [Function(nameof(EligibilityArchivalFunction))]
    public async Task Run([TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("EligibilityArchivalFunction triggered at {Time}", DateTime.UtcNow);

        const string archiveSql = """
            INSERT INTO CAN_ELIGIBILITY_MASTER_HIS (CN_COM_COD, CN_SFT_COD, CN_ITM_COD, CN_ELG_LMT, CN_MOD_USR, CN_MOD_DAT)
            SELECT CN_COM_COD, CN_SFT_COD, CN_ITM_COD, CN_ELG_LMT, CN_ENT_USR, GETUTCDATE()
            FROM   CAN_ELIGIBILITY_MASTER
            WHERE  CN_ENT_DAT < DATEADD(YEAR, -1, GETUTCDATE());
            """;

        try
        {
            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(archiveSql, conn);
            var rows = await cmd.ExecuteNonQueryAsync();
            _logger.LogInformation("EligibilityArchivalFunction archived {Rows} record(s).", rows);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "EligibilityArchivalFunction failed.");
            throw;
        }
    }
}
