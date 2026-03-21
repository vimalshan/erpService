using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace TravelService.Functions;

/// <summary>
/// Timer-triggered Azure Function to close approved tour plans older than 30 days.
/// Runs every day at 01:00 UTC.
/// </summary>
public class TourPlanClosureFunction
{
    private readonly ILogger<TourPlanClosureFunction> _logger;
    private readonly string _connectionString;

    public TourPlanClosureFunction(ILogger<TourPlanClosureFunction> logger, IConfiguration configuration)
    {
        _logger = logger;
        _connectionString = configuration.GetConnectionString("TravelDb")
            ?? throw new InvalidOperationException("TravelDb connection string not configured.");
    }

    [Function("TourPlanClosureFunction")]
    public async Task RunAsync([TimerTrigger("0 0 1 * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("TourPlanClosureFunction started at {Time}", DateTime.UtcNow);

        try
        {
            using var connection = new SqlConnection(_connectionString);
            var rowsAffected = await connection.ExecuteAsync(
                @"UPDATE TOURPLAN_MAIN 
                  SET TP_STATUS = 'Closed', TP_CLOSURESTATUS = 'C', TP_LASTMODIFIEDON = GETUTCDATE()
                  WHERE TP_STATUS = 'Approved' 
                    AND TP_ENDDATE < DATEADD(day, -30, GETUTCDATE())
                    AND (TP_CLOSURESTATUS IS NULL OR TP_CLOSURESTATUS <> 'C')");

            _logger.LogInformation("TourPlanClosureFunction: {Count} tour plans auto-closed.", rowsAffected);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during tour plan auto-closure.");
            throw;
        }
    }
}
