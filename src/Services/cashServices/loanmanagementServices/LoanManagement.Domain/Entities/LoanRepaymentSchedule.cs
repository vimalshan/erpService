using LoanManagement.Domain.Common;
using LoanManagement.Domain.Enums;

namespace LoanManagement.Domain.Entities;

public class LoanRepaymentSchedule : BaseEntity
{
    public long RepayId { get; private set; }
    public decimal? RepayLoanId { get; private set; }
    public DateTime? RepayDate { get; private set; }
    public decimal? RepayAmt { get; private set; }
    public string? RepayFlag { get; private set; }   // O or A
    public DateTime? RepayModifiedOn { get; private set; }
    public long? RepayModifiedBy { get; private set; }

    private LoanRepaymentSchedule() { }

    public static LoanRepaymentSchedule Create(
        long repayId,
        decimal loanId,
        DateTime repayDate,
        decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Repayment amount must be positive.", nameof(amount));

        return new LoanRepaymentSchedule
        {
            RepayId = repayId,
            RepayLoanId = loanId,
            RepayDate = repayDate,
            RepayAmt = amount,
            RepayFlag = "O"
        };
    }

    public void Amend(decimal newAmount, long modifiedBy)
    {
        RepayAmt = newAmount;
        RepayFlag = "A";
        RepayModifiedBy = modifiedBy;
        RepayModifiedOn = DateTime.UtcNow;
    }

    public bool IsOriginal => RepayFlag == "O";
    public bool IsAmended => RepayFlag == "A";
}
