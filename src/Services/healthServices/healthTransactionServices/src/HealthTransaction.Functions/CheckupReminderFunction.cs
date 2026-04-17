using Dapper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HealthTransaction.Functions;

/// <summary>
/// Runs daily at midnight — logs upcoming checkups for the next 7 days.
/// </summary>
public class CheckupReminderFunction
{
    private readonly SqlConnection _connection;
    private readonly ILogger<CheckupReminderFunction> _logger;

    public CheckupReminderFunction(SqlConnection connection, ILogger<CheckupReminderFunction> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    [Function("CheckupReminder")]
    public async Task Run([TimerTrigger("0 0 0 * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation("CheckupReminder triggered at {Time}", DateTime.UtcNow);

        var upcoming = await _connection.QueryAsync<dynamic>(
            @"SELECT CPM_EMP_NUM, CPM_COM_COD, CPM_CHK_DAT
              FROM CHKUP_PRE_MAIN
              WHERE CPM_CHK_DAT BETWEEN @From AND @To",
            new { From = DateTime.Today, To = DateTime.Today.AddDays(7) });

        foreach (var row in upcoming)
            _logger.LogInformation("Upcoming checkup — EmpNum: {EmpNum}, Date: {Date}", (object)row.CPM_EMP_NUM, (object)row.CPM_CHK_DAT);

        _logger.LogInformation("CheckupReminder completed. Checked {Count} records.", upcoming.Count());
    }
}
