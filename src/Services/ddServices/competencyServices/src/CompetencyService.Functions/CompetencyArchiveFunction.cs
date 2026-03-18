using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using Dapper;

namespace CompetencyService.Functions;

/// <summary>
/// Timer-triggered Azure Function: archives expired competencies to *_DEL tables daily.
/// </summary>
public class CompetencyArchiveFunction(ILogger<CompetencyArchiveFunction> logger, IConfiguration config)
{
    // Runs daily at 02:00 UTC
    [Function(nameof(CompetencyArchiveFunction))]
    public async Task Run([TimerTrigger("0 0 2 * * *")] TimerInfo timerInfo)
    {
        logger.LogInformation("CompetencyArchiveFunction started at {Time}", DateTime.UtcNow);

        var connectionString = config.GetConnectionString("CompetencyDb")
            ?? throw new InvalidOperationException("CompetencyDb connection string not found.");

        using var conn = new SqlConnection(connectionString);

        // Archive expired role-specific competencies
        const string archiveRoleSpecific = """
            INSERT INTO ROLE_SPECIFIC_DEL
                SELECT * FROM ROLE_SPECIFIC WHERE EFF_TO < @Now AND EFF_TO IS NOT NULL
            DELETE FROM ROLE_SPECIFIC WHERE EFF_TO < @Now AND EFF_TO IS NOT NULL
            """;

        var affected = await conn.ExecuteAsync(archiveRoleSpecific, new { Now = DateTime.UtcNow });
        logger.LogInformation("Archived {Count} expired RoleSpecific records.", affected);
    }
}
