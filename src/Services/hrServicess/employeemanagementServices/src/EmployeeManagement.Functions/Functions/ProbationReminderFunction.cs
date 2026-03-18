using EmployeeManagement.Application.Common.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Functions.Functions;

/// <summary>
/// Timer-triggered Azure Function that checks for overdue probations daily at 8 AM
/// and publishes reminder events.
/// </summary>
public class ProbationReminderFunction(IDapperQueryService dapperQueryService,
    IMessagePublisher messagePublisher,
    ILogger<ProbationReminderFunction> logger)
{
    // Runs every day at 08:00 UTC
    [Function(nameof(ProbationReminderFunction))]
    public async Task Run([TimerTrigger("0 0 8 * * *")] TimerInfo timerInfo)
    {
        logger.LogInformation("ProbationReminderFunction triggered at: {Time}", DateTimeOffset.UtcNow);

        try
        {
            const string sql = """
                SELECT PROBATION_ID, PROBATION_EMP_SYSID, PROBATION_ENDDATE
                FROM   EMP_PROBATIONKK
                WHERE  PROBATION_STATUS = 'A'
                  AND  PROBATION_ENDDATE < GETDATE()
                """;

            var overdue = await dapperQueryService.QueryAsync<OverdueProbationRecord>(sql, null);

            foreach (var record in overdue)
            {
                var message = new
                {
                    EventType = "ProbationOverdue",
                    record.ProbationId,
                    record.EmployeeSysId,
                    record.EndDate,
                    DetectedAt = DateTimeOffset.UtcNow
                };

                await messagePublisher.PublishAsync("hr.exchange", "hr.probation.overdue", message);

                logger.LogWarning(
                    "Probation overdue — ProbationId={ProbationId}, EmployeeSysId={EmployeeSysId}",
                    record.ProbationId, record.EmployeeSysId);
            }

            logger.LogInformation("ProbationReminderFunction completed. Overdue count: {Count}", overdue.Count());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in ProbationReminderFunction");
            throw;
        }
    }

    private sealed record OverdueProbationRecord(int ProbationId, int EmployeeSysId, DateTime EndDate);
}
