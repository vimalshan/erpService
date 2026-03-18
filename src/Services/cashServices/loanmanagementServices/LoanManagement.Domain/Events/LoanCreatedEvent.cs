using LoanManagement.Domain.Common;

namespace LoanManagement.Domain.Events;

public class LoanCreatedEvent : BaseEvent
{
    public decimal LoanId { get; }
    public string LoanKey { get; }
    public decimal LoanAmount { get; }

    public LoanCreatedEvent(decimal loanId, string loanKey, decimal loanAmount)
    {
        LoanId = loanId;
        LoanKey = loanKey;
        LoanAmount = loanAmount;
    }
}
