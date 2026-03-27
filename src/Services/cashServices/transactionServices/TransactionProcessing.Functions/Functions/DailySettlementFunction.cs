using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TransactionProcessing.Domain.Interfaces;

namespace TransactionProcessing.Functions.Functions;

public sealed class DailySettlementFunction(
    ILoggerFactory loggerFactory,
    IUnitOfWork unitOfWork,
    IEventPublisher eventPublisher)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<DailySettlementFunction>();

    [Function("DailySettlementProcess")]
    public async Task Run([TimerTrigger("0 0 18 * * *")] TimerInfo timer, CancellationToken ct)
    {
        _logger.LogInformation("Daily settlement process started at {Time}", DateTime.UtcNow);

        var openBatches = await unitOfWork.Batches.GetByStatusAsync("OPEN", ct);

        foreach (var batch in openBatches.Where(b => b.BatchDate.Date <= DateTime.UtcNow.Date))
        {
            var txns = await unitOfWork.Transactions.GetByBatchIdAsync(batch.BatchId, ct);
            int success = txns.Count(t => t.TxnStatus == "COMPLETED");
            int failure = txns.Count(t => t.TxnStatus == "FAILED");
            decimal total = txns.Where(t => t.TxnStatus == "COMPLETED").Sum(t => t.TxnBaseAmount ?? 0m);

            batch.Complete(success, failure, total);

            foreach (var evt in batch.DomainEvents)
                await eventPublisher.PublishAsync(evt, "transaction.batch.completed", ct);

            batch.ClearDomainEvents();
        }

        await unitOfWork.SaveChangesAsync(ct);
        _logger.LogInformation("Daily settlement process completed. Processed {Count} batches", openBatches.Count);
    }
}
