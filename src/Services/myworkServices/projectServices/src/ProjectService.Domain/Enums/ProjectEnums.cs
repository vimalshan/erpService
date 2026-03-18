namespace ProjectService.Domain.Enums;

public enum ProjectStatus
{
    Active = 'A',
    Closed = 'C',
    OnHold = 'H',
    Dropped = 'D',
    Pending = 'P'
}

public enum ApprovalType
{
    CharterClose = 'C',
    CharterApproval = 'A',
    Drop = 'D'
}

public enum ApprovalStatus
{
    Pending = 'P',
    Approved = 'A',
    Rejected = 'R'
}

public enum HoldType
{
    Hold = 'H',
    Unhold = 'U'
}

public enum AccessType
{
    Admin = 'A',
    ReadOnly = 'R',
    Write = 'W'
}
