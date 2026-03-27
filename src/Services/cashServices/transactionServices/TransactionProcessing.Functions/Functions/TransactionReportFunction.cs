using System.Text;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using TransactionProcessing.Domain.Interfaces;

namespace TransactionProcessing.Functions.Functions;

public sealed class TransactionReportFunction(
    ILoggerFactory loggerFactory,
    IUnitOfWork unitOfWork,
    IBlobStorageService blobStorage)
{
    private readonly ILogger _logger = loggerFactory.CreateLogger<TransactionReportFunction>();

    [Function("GenerateTransactionReport")]
    public async Task Run([TimerTrigger("0 0 6 * * *")] TimerInfo timer, CancellationToken ct)
    {
        _logger.LogInformation("Transaction report generation started at {Time}", DateTime.UtcNow);

        var yesterday = DateTime.UtcNow.Date.AddDays(-1);
        var txns = await unitOfWork.Transactions.GetByDateRangeAsync(yesterday, yesterday.AddDays(1), ct);

        var report = new
        {
            ReportDate = yesterday,
            GeneratedAt = DateTime.UtcNow,
            TotalTransactions = txns.Count,
            CompletedCount = txns.Count(t => t.TxnStatus == "COMPLETED"),
            FailedCount = txns.Count(t => t.TxnStatus == "FAILED"),
            TotalAmount = txns.Where(t => t.TxnStatus == "COMPLETED").Sum(t => t.TxnBaseAmount),
            ByType = txns.GroupBy(t => t.TxnType).Select(g => new
            {
                Type = g.Key,
                Count = g.Count(),
                Amount = g.Sum(t => t.TxnBaseAmount)
            })
        };

        var json = JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true });
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
        var blobName = $"daily-reports/{yesterday:yyyy/MM/dd}/transaction-report.json";
        await blobStorage.UploadAsync("transaction-reports", blobName, stream, ct);

        _logger.LogInformation("Report generated: {TotalTxns} transactions, {TotalAmount:C} total",
            report.TotalTransactions, report.TotalAmount);
    }
}
