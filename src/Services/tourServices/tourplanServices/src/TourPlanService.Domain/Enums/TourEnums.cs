namespace TourPlanService.Domain.Enums;

public enum TourStatus
{
    Draft = 0,
    Submitted = 1,
    PendingApproval = 2,
    Approved = 3,
    Rejected = 4,
    Cancelled = 5,
    Completed = 6,
    ExpenseSubmitted = 7,
    ExpenseApproved = 8,
    Closed = 9
}

public enum TravelCategory
{
    Domestic = 0,   // DOM
    International = 1  // INT
}

public enum TourType
{
    SingleCity = 0,
    MultipleCity = 1
}

public enum ClaimType
{
    Flat = 0,
    Actuals = 1,
    Combination = 2
}

public enum AdvanceType
{
    Normal = 0,     // N
    Additional = 1  // A
}

public enum ForexRequestType
{
    Request = 0,     // R
    Surrender = 1    // S
}

public enum ApprovalStatus
{
    Pending = 0,
    Approved = 1,
    Rejected = 2
}
