namespace DealTicketing.Domain.Enums;

public enum DealTransactionType
{
    Buy = 'B',
    Sell = 'S',
    Put = 'P',
    Call = 'C'
}

public enum DealApprovalStatus
{
    Confirmed = 'Y',
    Pending = 'P',
    Rejected = 'R',
    PendingConfirmation = 'N'
}

public enum SettlementType
{
    Utilized = 'U',
    Cancelled = 'C',
    Rollover = 'R'
}

public enum SettlementStatus
{
    Live = 'L',
    Closed = 'C'
}
