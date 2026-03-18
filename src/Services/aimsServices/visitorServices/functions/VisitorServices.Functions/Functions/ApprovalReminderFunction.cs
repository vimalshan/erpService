using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using VisitorServices.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using VisitorServices.Domain.Enums;

namespace VisitorServices.Functions.Functions;

/// <summary>
/// Runs every 30 minutes and sends reminders for approval requests pending > 1 hour.
/// </summary>
public class ApprovalReminderFunction(ILogger<ApprovalReminderFunction> logger, VisitorDbContext dbContext)
{
    [Function("ApprovalReminder")]
    public async Task Run(
        [TimerTrigger("0 */30 * * * *")] TimerInfo timerInfo,  // every 30 min
        CancellationToken cancellationToken)
    {
        logger.LogInformation("ApprovalReminder triggered at {Time}", DateTime.UtcNow);

        var threshold = DateTime.UtcNow.AddHours(-1);

        var pendingRequests = await dbContext.ApprovalRequests
            .Where(r => r.ApprovalStatus == ApprovalStatus.Pending && r.RequestedOn < threshold)
            .ToListAsync(cancellationToken);

        foreach (var request in pendingRequests)
        {
            // Integration point: fire notification to approver
            logger.LogWarning(
                "Approval request {RequestId} for visitor {VisitorId} has been pending since {RequestedOn}. Approver: {ApproverId}",
                request.Id, request.VisitorId, request.RequestedOn, request.RequiredApproverId);
        }

        logger.LogInformation("Checked {Count} overdue approval request(s).", pendingRequests.Count);
    }
}
