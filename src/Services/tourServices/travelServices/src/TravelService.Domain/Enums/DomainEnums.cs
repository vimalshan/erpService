namespace TravelService.Domain.Enums;

public enum TourPlanStatus
{
    Draft = 0,
    Submitted = 1,
    PendingApproval = 2,
    Approved = 3,
    Rejected = 4,
    Cancelled = 5,
    Closed = 6
}

public enum TravelCategory
{
    Domestic = 0,
    International = 1
}

public enum BatchStatus
{
    Created = 0,
    PendingAdminApproval = 1,
    AdminApproved = 2,
    PendingFinanceApproval = 3,
    FinanceApproved = 4,
    Rejected = 5,
    Cancelled = 6
}

public enum ApprovalStatus
{
    Pending = 0,
    Approved = 1,
    Rejected = 2,
    Cancelled = 3
}

public enum ForexRequestType
{
    Request = 0,
    Surrender = 1
}
