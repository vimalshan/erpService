using ComplaintService.Application.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ComplaintService.Functions;

/// <summary>
/// Timer-triggered Azure Function that checks for complaints past their target date
/// and triggers escalation logic.
/// Runs every hour: "0 0 * * * *"
/// </summary>
public class ComplaintEscalationFunction(
    ILogger<ComplaintEscalationFunction> logger,
    IComplaintRepository complaintRepo,
    IMessagePublisher publisher)
{
    [Function("ComplaintEscalationCheck")]
    public async Task Run([TimerTrigger("0 0 * * * *")] TimerInfo timer, CancellationToken ct)
    {
        logger.LogInformation("Complaint escalation check started at {UtcNow}", DateTime.UtcNow);

        var page = 1;
        const int pageSize = 100;

        while (true)
        {
            var tickets = (await complaintRepo.GetAllAsync(page, pageSize, ct)).ToList();
            if (tickets.Count == 0) break;

            foreach (var ticket in tickets.Where(t => !t.IsClosed))
            {
                if (!DateTime.TryParse(ticket.TargetDate, out var targetDate)) continue;
                if (DateTime.UtcNow <= targetDate) continue;

                var hoursOverdue = (decimal)(DateTime.UtcNow - targetDate).TotalHours;
                logger.LogWarning("Ticket {TicketNum} is {Hours}h overdue", ticket.TicketNum, hoursOverdue);

                await publisher.PublishAsync(
                    new { ticket.TicketNum, HoursOverdue = hoursOverdue, CheckedAt = DateTime.UtcNow },
                    "complaint.escalation.check", ct);
            }

            if (tickets.Count < pageSize) break;
            page++;
        }

        logger.LogInformation("Complaint escalation check completed.");
    }
}
