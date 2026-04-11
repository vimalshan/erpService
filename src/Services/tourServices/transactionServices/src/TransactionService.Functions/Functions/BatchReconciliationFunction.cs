using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TransactionService.Infrastructure.Persistence;

namespace TransactionService.Functions.Functions;

/// <summary>
/// Reconciles travel batch totals against sub-item totals, flagging mismatches.
/// Schedule: Every day at 4:00 AM UTC
/// </summary>
public sealed class BatchReconciliationFunction
{
    private readonly TransactionDbContext _context;
    private readonly ILogger<BatchReconciliationFunction> _logger;

    public BatchReconciliationFunction(TransactionDbContext context,
        ILogger<BatchReconciliationFunction> logger)
    {
        _context = context;
        _logger = logger;
    }

    [Function(nameof(BatchReconciliationFunction))]
    public async Task Run([TimerTrigger("0 0 4 * * *")] TimerInfo timerInfo, CancellationToken ct)
    {
        _logger.LogInformation("Batch reconciliation started at {Time}", DateTime.UtcNow);

        var pendingBatches = await _context.TravelBatches
            .Include(b => b.SubItems)
            .Where(b => b.Status == "P")
            .ToListAsync(ct);

        var mismatchCount = 0;

        foreach (var batch in pendingBatches)
        {
            var subTotal = batch.SubItems
                .Sum(s => decimal.TryParse(s.TotAmt, out var amt) ? amt : 0m);
            if (decimal.TryParse(batch.BillAmount, out var headerTotal)
                && Math.Abs(headerTotal - subTotal) > 0.01m)
            {
                mismatchCount++;
                _logger.LogWarning(
                    "Batch {BatchId} total mismatch: Header={HeaderTotal}, SubItems={SubTotal}",
                    batch.BatchId, headerTotal, subTotal);
            }
        }

        _logger.LogInformation(
            "Batch reconciliation finished. {Total} pending batches checked, {Mismatches} mismatches found.",
            pendingBatches.Count, mismatchCount);
    }
}
