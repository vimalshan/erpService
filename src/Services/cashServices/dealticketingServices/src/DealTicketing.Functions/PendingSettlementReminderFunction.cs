using DealTicketing.Infrastructure.Persistence;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DealTicketing.Functions;

/// <summary>
/// Timer-triggered function that runs every day at 9 AM UTC
/// to send reminders for deals pending settlement.
/// </summary>
public class PendingSettlementReminderFunction(
    DealTicketingDbContext dbContext,
    ILogger<PendingSettlementReminderFunction> logger)
{
    [Function(nameof(PendingSettlementReminderFunction))]
    public async Task Run(
        [TimerTrigger("0 0 9 * * *")] TimerInfo timerInfo,
        CancellationToken ct)
    {
        logger.LogInformation("PendingSettlementReminderFunction triggered at {Time}", DateTime.UtcNow);

        var pendingSettlements = await dbContext.DealDetails
            .Where(d => d.DealAppStatus == 'Y'
                        && (d.DealSetStatus == null || d.DealSetStatus == 'L')
                        && d.DealMatDate.HasValue
                        && d.DealMatDate.Value <= DateTime.UtcNow)
            .Select(d => new { d.DealId, d.DealMatDate, d.DealAmount, d.DealBatchId })
            .ToListAsync(ct);

        logger.LogInformation(
            "Found {Count} past-maturity deals without settlement.",
            pendingSettlements.Count);

        // TODO: push notifications / email to settlement team
        foreach (var deal in pendingSettlements)
        {
            logger.LogWarning(
                "Overdue settlement: DealId={DealId}, BatchId={BatchId}, Amount={Amount}, MatDate={MatDate}",
                deal.DealId, deal.DealBatchId, deal.DealAmount, deal.DealMatDate);
        }
    }
}

/// <summary>
/// HTTP-triggered function for manually triggering deal report generation.
/// </summary>
public class GenerateDealReportFunction(
    DealTicketingDbContext dbContext,
    ILogger<GenerateDealReportFunction> logger)
{
    [Function(nameof(GenerateDealReportFunction))]
    public async Task<IEnumerable<object>> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "reports/daily")] Microsoft.Azure.Functions.Worker.Http.HttpRequestData req,
        CancellationToken ct)
    {
        var fromDateStr = req.Query["fromDate"];
        var toDateStr = req.Query["toDate"];

        DateTime fromDate = DateTime.TryParse(fromDateStr, out var fd) ? fd : DateTime.UtcNow.AddDays(-7);
        DateTime toDate = DateTime.TryParse(toDateStr, out var td) ? td : DateTime.UtcNow;

        logger.LogInformation("Generating deal report from {From} to {To}", fromDate, toDate);

        var deals = await dbContext.DealBatches
            .Include(b => b.Bank)
            .Include(b => b.DealDetails)
            .Where(b => b.DealDate >= fromDate && b.DealDate <= toDate)
            .Select(b => new
            {
                b.DealBatchId,
                b.DealDate,
                BankName = b.Bank != null ? b.Bank.BankName : "N/A",
                DealCount = b.DealDetails.Count,
                TotalAmount = b.DealDetails.Sum(d => (decimal?)d.DealAmount ?? 0)
            })
            .ToListAsync(ct);

        return deals;
    }
}
