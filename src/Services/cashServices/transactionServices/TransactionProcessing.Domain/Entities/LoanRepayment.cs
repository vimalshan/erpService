using TransactionProcessing.Domain.Common;
using TransactionProcessing.Domain.Events;

namespace TransactionProcessing.Domain.Entities;

public class LoanRepayment : BaseEntity
{
    public long RepayProcId { get; private set; }
    public long TxnId { get; private set; }
    public long LoanId { get; private set; }
    public long RepayId { get; private set; }
    public decimal RepayAmount { get; private set; }
    public decimal? ExchangeRate { get; private set; }
    public decimal? ConvertedAmount { get; private set; }
    public long? BankAccountId { get; private set; }
    public string ProcessingStatus { get; private set; } = "PENDING";
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }

    public FinancialTransaction? Transaction { get; private set; }

    private LoanRepayment() { }

    public static LoanRepayment Create(
        long txnId, long loanId, long repayId, decimal repayAmount,
        decimal? exchangeRate, long? bankAccountId, long createdBy)
    {
        var repay = new LoanRepayment
        {
            TxnId = txnId,
            LoanId = loanId,
            RepayId = repayId,
            RepayAmount = repayAmount,
            ExchangeRate = exchangeRate,
            ConvertedAmount = exchangeRate.HasValue ? repayAmount * exchangeRate.Value : repayAmount,
            BankAccountId = bankAccountId,
            ProcessingStatus = "PENDING",
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };

        repay.AddDomainEvent(new RepaymentProcessedEvent(repay.RepayProcId, loanId, repayId, repayAmount));
        return repay;
    }

    public void MarkProcessed() => ProcessingStatus = "COMPLETED";
    public void MarkFailed() => ProcessingStatus = "FAILED";
}
