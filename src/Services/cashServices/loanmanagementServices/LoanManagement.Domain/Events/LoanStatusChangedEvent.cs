using LoanManagement.Domain.Common;

namespace LoanManagement.Domain.Events;

public class LoanStatusChangedEvent : BaseEvent
{
    public decimal LoanId { get; }
    public string NewStatus { get; }

    public LoanStatusChangedEvent(decimal loanId, string newStatus)
    {
        LoanId = loanId;
        NewStatus = newStatus;
    }
}
