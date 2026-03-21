using FinanceService.Application.Common.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinanceService.AzureFunctions.Functions;

public class SyncPaymentStatusFunction
{
    private readonly IFinanceDbContext _context;
    private readonly ILogger<SyncPaymentStatusFunction> _logger;

    public SyncPaymentStatusFunction(IFinanceDbContext context, ILogger<SyncPaymentStatusFunction> logger)
    {
        _context = context;
        _logger = logger;
    }

    [Function("SyncPaymentStatus")]
    public async Task Run([TimerTrigger("0 */30 * * * *")] TimerInfo timer)
    {
        _logger.LogInformation("Syncing payment status at: {Time}", DateTime.UtcNow);

        var paymentInProgressBatches = await _context.TravelBatchMains
            .Where(b => b.BatchStatus == "P")
            .ToListAsync();

        foreach (var batch in paymentInProgressBatches)
        {
            var hasPayment = await _context.TravelAccounts
                .AnyAsync(a => a.Remarks != null && a.Remarks.Contains(batch.BatchNumber.ToString()));

            if (hasPayment)
            {
                batch.BatchStatus = "C"; // Completed
                _logger.LogInformation("Batch {UnitCode}-{BatchNumber} marked as completed",
                    batch.UnitCode, batch.BatchNumber);
            }
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Payment status sync completed.");
    }
}
