using LoanManagement.Domain.Common;

namespace LoanManagement.Domain.Events;

public class LoanDisbursedEvent : BaseEvent
{
    public decimal LoanId { get; }
    public long DisbursementId { get; }
    public decimal Amount { get; }

    public LoanDisbursedEvent(decimal loanId, long disbursementId, decimal amount)
    {
        LoanId = loanId;
        DisbursementId = disbursementId;
        Amount = amount;
    }
}
