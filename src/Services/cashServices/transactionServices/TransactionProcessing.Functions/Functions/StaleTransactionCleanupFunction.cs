using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TransactionProcessing.Domain.Entities;
using TransactionProcessing.Domain.Interfaces;

namespace TransactionProcessing.Functions.Functions;

public sealed class StaleTransactionCleanupFunction(
    ILoggerFactory loggerFactory,
    IUnitOfWork unitOfWork)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<StaleTransactionCleanupFunction>();

    [Function("StaleTransactionCleanup")]
    public async Task Run([TimerTrigger("0 0 2 * * *")] TimerInfo timer, CancellationToken ct)
    {
        _logger.LogInformation("Stale transaction cleanup started at {Time}", DateTime.UtcNow);

        var staleTxns = await unitOfWork.Transactions.GetByStatusAsync("PROCESSING", ct);
        var staleThreshold = DateTime.UtcNow.AddHours(-24);
        int cleaned = 0;

        foreach (var txn in staleTxns.Where(t => t.CreatedOn < staleThreshold))
        {
            txn.MarkFailed("Transaction timed out after 24 hours", 0);
            cleaned++;
        }

        if (cleaned > 0)
            await unitOfWork.SaveChangesAsync(ct);

        _logger.LogInformation("Stale transaction cleanup completed. Cleaned {Count} transactions", cleaned);
    }
}
