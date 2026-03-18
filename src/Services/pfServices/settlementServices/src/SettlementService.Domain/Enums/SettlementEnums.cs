namespace SettlementService.Domain.Enums;

public enum SettlementStatus
{
    Pending = 'P',
    Approved = 'A',
    Completed = 'C',
    Rejected = 'R'
}

public enum PaymentStatus
{
    Pending = 'P',
    Completed = 'C',
    Failed = 'F'
}

public enum ApprovalStatus
{
    Pending = 'P',
    Approved = 'A',
    Rejected = 'R'
}
