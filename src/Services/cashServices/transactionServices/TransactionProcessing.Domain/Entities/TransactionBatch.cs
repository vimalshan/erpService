using TransactionProcessing.Domain.Common;
using TransactionProcessing.Domain.Events;

namespace TransactionProcessing.Domain.Entities;

public class TransactionBatch : AggregateRoot
{
    public long BatchId { get; private set; }
    public string BatchType { get; private set; } = string.Empty; // DAILY_SETTLEMENT, LOAN_PROCESSING, MANUAL
    public DateTime BatchDate { get; private set; }
    public string BatchStatus { get; private set; } = "OPEN";    // OPEN, PROCESSING, COMPLETED, FAILED
    public int? BatchTotalCount { get; private set; }
    public int? BatchSuccessCount { get; private set; }
    public int? BatchFailureCount { get; private set; }
    public decimal? BatchTotalAmount { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public DateTime? CompletedOn { get; private set; }

    public ICollection<FinancialTransaction> Transactions { get; private set; } = new List<FinancialTransaction>();

    private TransactionBatch() { }

    public static TransactionBatch Create(string batchType, DateTime batchDate, long createdBy)
    {
        return new TransactionBatch
        {
            BatchType = batchType,
            BatchDate = batchDate,
            BatchStatus = "OPEN",
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };
    }

    public void StartProcessing()
    {
        BatchStatus = "PROCESSING";
        BatchTotalCount = Transactions.Count;
    }

    public void Complete(int successCount, int failureCount, decimal totalAmount)
    {
        BatchStatus = "COMPLETED";
        BatchSuccessCount = successCount;
        BatchFailureCount = failureCount;
        BatchTotalAmount = totalAmount;
        CompletedOn = DateTime.UtcNow;
        AddDomainEvent(new BatchCompletedEvent(BatchId, BatchType, successCount, failureCount, totalAmount));
    }

    public void MarkFailed()
    {
        BatchStatus = "FAILED";
        CompletedOn = DateTime.UtcNow;
    }
}
