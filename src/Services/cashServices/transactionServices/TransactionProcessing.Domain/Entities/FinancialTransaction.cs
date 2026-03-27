using TransactionProcessing.Domain.Common;
using TransactionProcessing.Domain.Events;

namespace TransactionProcessing.Domain.Entities;

public class FinancialTransaction : AggregateRoot
{
    public long TxnId { get; private set; }
    public long? TxnBatchId { get; private set; }
    public string TxnType { get; private set; } = string.Empty;       // SETTLEMENT, DISBURSEMENT, REPAYMENT, CASH_TRANSFER
    public string? TxnSubType { get; private set; }                    // UTILIZED, CANCELLED, ROLLOVER, etc.
    public decimal TxnAmount { get; private set; }
    public long? TxnCurrencyId { get; private set; }
    public decimal? TxnExchangeRate { get; private set; }
    public decimal? TxnBaseAmount { get; private set; }
    public string? TxnReference { get; private set; }
    public string TxnSourceService { get; private set; } = string.Empty; // CashManagement, DealTicketing, LoanManagement
    public long? TxnSourceId { get; private set; }
    public string TxnStatus { get; private set; } = "PENDING";        // PENDING, PROCESSING, COMPLETED, FAILED, REVERSED
    public string? TxnRemarks { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long? UpdatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }

    // Navigation
    public TransactionBatch? Batch { get; private set; }
    public DealSettlement? DealSettlement { get; private set; }
    public LoanDisbursement? LoanDisbursement { get; private set; }
    public LoanRepayment? LoanRepayment { get; private set; }
    public ICollection<TransactionAudit> Audits { get; private set; } = new List<TransactionAudit>();

    private FinancialTransaction() { }

    public static FinancialTransaction Create(
        string txnType, string? txnSubType, decimal amount,
        long? currencyId, decimal? exchangeRate, string? reference,
        string sourceService, long? sourceId, string? remarks, long createdBy)
    {
        var txn = new FinancialTransaction
        {
            TxnType = txnType,
            TxnSubType = txnSubType,
            TxnAmount = amount,
            TxnCurrencyId = currencyId,
            TxnExchangeRate = exchangeRate,
            TxnBaseAmount = exchangeRate.HasValue ? amount * exchangeRate.Value : amount,
            TxnReference = reference,
            TxnSourceService = sourceService,
            TxnSourceId = sourceId,
            TxnStatus = "PENDING",
            TxnRemarks = remarks,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };

        txn.AddDomainEvent(new TransactionRecordedEvent(txn.TxnId, txnType, amount, sourceService));
        return txn;
    }

    public void AssignToBatch(long batchId)
    {
        TxnBatchId = batchId;
        UpdatedOn = DateTime.UtcNow;
    }

    public void MarkProcessing(long updatedBy)
    {
        var previous = TxnStatus;
        TxnStatus = "PROCESSING";
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
        Audits.Add(TransactionAudit.Create(TxnId, previous, TxnStatus, "Status changed to PROCESSING", updatedBy));
    }

    public void MarkCompleted(long updatedBy)
    {
        var previous = TxnStatus;
        TxnStatus = "COMPLETED";
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
        Audits.Add(TransactionAudit.Create(TxnId, previous, TxnStatus, "Transaction completed", updatedBy));
        AddDomainEvent(new TransactionRecordedEvent(TxnId, TxnType, TxnAmount, TxnSourceService));
    }

    public void MarkFailed(string reason, long updatedBy)
    {
        var previous = TxnStatus;
        TxnStatus = "FAILED";
        TxnRemarks = reason;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
        Audits.Add(TransactionAudit.Create(TxnId, previous, TxnStatus, $"Failed: {reason}", updatedBy));
    }

    public void Reverse(long updatedBy)
    {
        var previous = TxnStatus;
        TxnStatus = "REVERSED";
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
        Audits.Add(TransactionAudit.Create(TxnId, previous, TxnStatus, "Transaction reversed", updatedBy));
    }
}
