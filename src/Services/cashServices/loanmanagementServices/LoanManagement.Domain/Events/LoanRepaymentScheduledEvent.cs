using LoanManagement.Domain.Common;

namespace LoanManagement.Domain.Events;

public class LoanRepaymentScheduledEvent : BaseEvent
{
    public decimal LoanId { get; }
    public long RepaymentId { get; }
    public DateTime RepayDate { get; }
    public decimal Amount { get; }

    public LoanRepaymentScheduledEvent(decimal loanId, long repaymentId, DateTime repayDate, decimal amount)
    {
        LoanId = loanId;
        RepaymentId = repaymentId;
        RepayDate = repayDate;
        Amount = amount;
    }
}
