namespace FinanceService.Domain.Enums;

public enum BatchStatus
{
    New = 'N',
    Approved = 'Y',
    PaymentInProgress = 'P',
    Completed = 'C',
    Rejected = 'R'
}
