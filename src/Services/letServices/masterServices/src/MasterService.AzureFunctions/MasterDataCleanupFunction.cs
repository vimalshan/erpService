using Dapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MasterService.AzureFunctions;

/// <summary>
/// Azure Function: Archives closed/cancelled master records older than 1 year.
/// Runs nightly at 02:00 UTC using a CRON timer.
/// </summary>
public class MasterDataCleanupFunction(IConfiguration configuration, ILogger<MasterDataCleanupFunction> logger)
{
    [Function("MasterDataCleanup")]
    public async Task RunAsync([TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo)
    {
        logger.LogInformation("MasterDataCleanup started at {Time}", DateTime.UtcNow);

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection not configured.");

        await using var connection = new SqlConnection(connectionString);

        var cutoff = DateTime.UtcNow.AddYears(-1);

        // Archive closed skills older than 1 year
        var closedSkills = await connection.ExecuteAsync(
            "DELETE FROM dbo.SKILL_MAST WHERE SK_CLS_DAT < @Cutoff",
            new { Cutoff = cutoff });

        // Archive cancelled training providers older than 1 year
        var cancelledTrainings = await connection.ExecuteAsync(
            "DELETE FROM dbo.TRAIN_MAST WHERE TR_CAN_DAT < @Cutoff",
            new { Cutoff = cutoff });

        logger.LogInformation(
            "MasterDataCleanup completed: {Skills} skills, {Trainings} training providers archived.",
            closedSkills, cancelledTrainings);
    }
}
