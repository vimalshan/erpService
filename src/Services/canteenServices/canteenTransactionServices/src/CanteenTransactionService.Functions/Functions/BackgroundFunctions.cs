using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using CanteenTransactionService.Domain.Interfaces;
using CanteenTransactionService.Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;

namespace CanteenTransactionService.Functions;

/// <summary>
/// Timer-triggered function that processes pending MIS batch submissions every 5 minutes.
/// </summary>
public sealed class ProcessPendingBatchesFunction
{
    private readonly CanteenTransactionDbContext _dbContext;
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<ProcessPendingBatchesFunction> _logger;

    public ProcessPendingBatchesFunction(
        CanteenTransactionDbContext dbContext,
        IMessagePublisher publisher,
        ILogger<ProcessPendingBatchesFunction> logger)
    {
        _dbContext = dbContext;
        _publisher = publisher;
        _logger = logger;
    }

    [Function(nameof(ProcessPendingBatchesFunction))]
    public async Task Run(
        [TimerTrigger("0 */5 * * * *")] TimerInfo timer,
        CancellationToken ct)
    {
        _logger.LogInformation("Processing pending MIS batches at {Time}", DateTime.UtcNow);

        var pending = await _dbContext.MisBatchSubmissions
            .Where(x => x.UpdateStatus == "P")
            .ToListAsync(ct);

        if (!pending.Any())
        {
            _logger.LogInformation("No pending MIS batches to process.");
            return;
        }

        foreach (var batch in pending)
        {
            try
            {
                batch.MarkAsProcessed();
                await _publisher.PublishAsync(
                    new
                    {
                        batch.SerialNumber,
                        batch.BatchNumber,
                        batch.CompanyCode,
                        batch.EmployeeNumber,
                        ProcessedAt = DateTime.UtcNow
                    },
                    "mis.batch.processed",
                    ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process MIS batch {Serial}", batch.SerialNumber);
                batch.MarkAsFailed();
            }
        }

        await _dbContext.SaveChangesAsync(ct);
        _logger.LogInformation("Processed {Count} MIS batch records.", pending.Count);
    }
}

/// <summary>
/// Timer-triggered function that aggregates daily canteen transactions at midnight.
/// </summary>
public sealed class DailyTransactionAggregationFunction
{
    private readonly CanteenTransactionDbContext _dbContext;
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<DailyTransactionAggregationFunction> _logger;

    public DailyTransactionAggregationFunction(
        CanteenTransactionDbContext dbContext,
        IMessagePublisher publisher,
        ILogger<DailyTransactionAggregationFunction> logger)
    {
        _dbContext = dbContext;
        _publisher = publisher;
        _logger = logger;
    }

    /// <summary>Runs daily at midnight to aggregate previous day's transactions.</summary>
    [Function(nameof(DailyTransactionAggregationFunction))]
    public async Task Run(
        [TimerTrigger("0 0 0 * * *")] TimerInfo timer,
        CancellationToken ct)
    {
        var yesterday = DateTime.UtcNow.AddDays(-1).ToString("yyyyMMdd");
        _logger.LogInformation("Aggregating transactions for date {Date}", yesterday);

        var transactions = await _dbContext.CanteenDacons
            .Where(x => x.SwipeDate == yesterday)
            .ToListAsync(ct);

        if (!transactions.Any())
        {
            _logger.LogInformation("No transactions found for {Date}.", yesterday);
            return;
        }

        var summary = transactions
            .GroupBy(t => new { t.CompanyCode, t.ItemCode })
            .Select(g => new
            {
                CompanyCode = g.Key.CompanyCode,
                ItemCode = g.Key.ItemCode,
                TotalQuantity = g.Sum(t => t.ItemQuantity),
                TransactionCount = g.Count(),
                Date = yesterday
            });

        foreach (var item in summary)
        {
            await _publisher.PublishAsync(item, "transaction.daily.summary", ct);
        }

        _logger.LogInformation("Published {Count} aggregation summaries for {Date}.", summary.Count(), yesterday);
    }
}
