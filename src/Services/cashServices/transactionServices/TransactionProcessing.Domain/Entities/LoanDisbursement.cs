using TransactionProcessing.Domain.Common;
using TransactionProcessing.Domain.Events;

namespace TransactionProcessing.Domain.Entities;

public class LoanDisbursement : BaseEntity
{
    public long DisbProcId { get; private set; }
    public long TxnId { get; private set; }
    public long LoanId { get; private set; }
    public long DisbId { get; private set; }
    public decimal DisbAmount { get; private set; }
    public decimal? ExchangeRate { get; private set; }
    public decimal? ConvertedAmount { get; private set; }
    public long? BankAccountId { get; private set; }
    public string ProcessingStatus { get; private set; } = "PENDING";
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }

    public FinancialTransaction? Transaction { get; private set; }

    private LoanDisbursement() { }

    public static LoanDisbursement Create(
        long txnId, long loanId, long disbId, decimal disbAmount,
        decimal? exchangeRate, long? bankAccountId, long createdBy)
    {
        var disb = new LoanDisbursement
        {
            TxnId = txnId,
            LoanId = loanId,
            DisbId = disbId,
            DisbAmount = disbAmount,
            ExchangeRate = exchangeRate,
            ConvertedAmount = exchangeRate.HasValue ? disbAmount * exchangeRate.Value : disbAmount,
            BankAccountId = bankAccountId,
            ProcessingStatus = "PENDING",
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };

        disb.AddDomainEvent(new DisbursementProcessedEvent(disb.DisbProcId, loanId, disbId, disbAmount));
        return disb;
    }

    public void MarkProcessed() => ProcessingStatus = "COMPLETED";
    public void MarkFailed() => ProcessingStatus = "FAILED";
}
