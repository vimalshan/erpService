namespace TravelRequestService.Domain.Enums;

public enum TravelRequestStatus
{
    Edit = 'E',
    Pending = 'N',
    Approved = 'A',
    Cancelled = 'C',
    Rejected = 'R',
    PendingExpenseApproval = 'F',
    PendingFinanceApproval = 'G',
    RejectedAtExpense = 'J',
    Settled = 'S'
}
