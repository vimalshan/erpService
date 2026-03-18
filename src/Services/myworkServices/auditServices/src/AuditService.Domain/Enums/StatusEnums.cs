namespace AuditService.Domain.Enums;

public enum AuditStatus
{
    Active = 'A',
    Inactive = 'I',
    Completed = 'C',
    Draft = 'D'
}

public enum ObservationStatus
{
    Pending = 'P',
    Revised = 'R',
    Completed = 'C'
}

public enum ApprovalStatus
{
    Pending = 'P',
    Approved = 'A',
    Rejected = 'R'
}

public enum UserType
{
    Admin = 'A',
    Auditor = 'U',
    Auditee = 'E'
}
