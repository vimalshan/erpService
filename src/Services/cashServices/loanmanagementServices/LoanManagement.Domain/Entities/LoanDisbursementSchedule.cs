using LoanManagement.Domain.Common;

namespace LoanManagement.Domain.Entities;

public class LoanDisbursementSchedule : BaseEntity
{
    public long DisbId { get; private set; }
    public decimal? DisbLoanId { get; private set; }
    public DateTime? DisbDate { get; private set; }
    public decimal? DisbAmount { get; private set; }
    public decimal? DisbExcRate { get; private set; }
    public decimal? DisbExcAmt { get; private set; }
    public long? DisbModifiedBy { get; private set; }
    public DateTime? DisbModifiedOn { get; private set; }

    private LoanDisbursementSchedule() { }

    public static LoanDisbursementSchedule Create(
        long disbId,
        decimal loanId,
        DateTime disbDate,
        decimal amount,
        decimal? excRate = null)
    {
        if (amount <= 0)
            throw new ArgumentException("Disbursement amount must be positive.", nameof(amount));

        return new LoanDisbursementSchedule
        {
            DisbId = disbId,
            DisbLoanId = loanId,
            DisbDate = disbDate,
            DisbAmount = amount,
            DisbExcRate = excRate,
            DisbExcAmt = excRate.HasValue ? amount * excRate.Value : amount
        };
    }

    public void Update(decimal amount, decimal? excRate, long modifiedBy)
    {
        DisbAmount = amount;
        DisbExcRate = excRate;
        DisbExcAmt = excRate.HasValue ? amount * excRate.Value : amount;
        DisbModifiedBy = modifiedBy;
        DisbModifiedOn = DateTime.UtcNow;
    }
}
