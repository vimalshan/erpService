using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RequestServices.Infrastructure.Data;

namespace RequestServices.Functions.Functions;

/// <summary>
/// Timer-triggered Azure Function that sends reminder notifications
/// for pending training requests older than 7 days.
/// Runs every morning at 07:00 UTC.
/// </summary>
public class PendingRequestReminderFunction(
    RequestDbContext context,
    ILogger<PendingRequestReminderFunction> logger)
{
    [Function(nameof(PendingRequestReminderFunction))]
    public async Task Run([TimerTrigger("0 0 7 * * *")] TimerInfo timerInfo)
    {
        logger.LogInformation("PendingRequestReminder triggered at {Now}", DateTime.UtcNow);

        var threshold = DateTime.UtcNow.AddDays(-7);

        var staleRequests = await context.RequestSub
            .Include(s => s.RequestMain)
            .Where(s => (s.StatusCode == 'P' || s.StatusCode == 'S')
                     && s.RequestDate < threshold)
            .ToListAsync();

        if (!staleRequests.Any())
        {
            logger.LogInformation("No pending requests requiring reminders.");
            return;
        }

        foreach (var sub in staleRequests)
        {
            // In a real implementation, send email/notification via Azure Communication Services.
            logger.LogWarning(
                "REMINDER: Request {RequestId}/{SerialNo} for employee {Employee} has been pending since {Date}.",
                sub.RequestId, sub.SerialNumber,
                sub.RequestMain?.EmployeeUser ?? "Unknown",
                sub.RequestDate);
        }

        logger.LogInformation("Sent reminders for {Count} stale requests.", staleRequests.Count);
    }
}
