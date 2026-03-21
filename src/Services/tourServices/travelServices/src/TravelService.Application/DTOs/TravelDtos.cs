namespace TravelService.Application.DTOs;

public class TourPlanDto
{
    public string Id { get; set; } = string.Empty;
    public string EmployeeSysId { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public string Remarks { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IncludeBookingRequests { get; set; }
    public string? TripType { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedOn { get; set; }
    public string FromCityId { get; set; } = string.Empty;
    public string FromCityName { get; set; } = string.Empty;
    public string ToCityId { get; set; } = string.Empty;
    public string ToCityName { get; set; } = string.Empty;
    public string SupervisorRemarks { get; set; } = string.Empty;
    public string? ContactNo { get; set; }
    public string? GradeType { get; set; }
    public string? PayrollUnitId { get; set; }
    public string? ClaimType { get; set; }
    public string? ApproverRemarks { get; set; }
    public string? ExpenseStatus { get; set; }
    public string? ClosureStatus { get; set; }
    public List<TourPlanAdvanceDto> Advances { get; set; } = new();
    public List<TourPlanAgendaDto> Agendas { get; set; } = new();
}

public class TourPlanAdvanceDto
{
    public string Id { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "INR";
    public string ApprovalStatus { get; set; } = string.Empty;
    public string Remarks { get; set; } = string.Empty;
}

public class TourPlanAgendaDto
{
    public string Id { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PartyToMeet { get; set; } = string.Empty;
    public string DesiredOutcome { get; set; } = string.Empty;
    public DateTime? AgendaDate { get; set; }
}

public class BatchMainDto
{
    public string Id { get; set; } = string.Empty;
    public string AdminId { get; set; } = string.Empty;
    public string PayrollUnitId { get; set; } = string.Empty;
    public DateTime BatchDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalPayable { get; set; }
    public string? InvoiceNo { get; set; }
    public DateTime? InvoiceDate { get; set; }
    public decimal InvoiceAmount { get; set; }
    public List<BatchSubDto> BatchSubs { get; set; } = new();
}

public class BatchSubDto
{
    public string Id { get; set; } = string.Empty;
    public string BatchId { get; set; } = string.Empty;
    public decimal BaseAmount { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal NetPayable { get; set; }
    public string CreditType { get; set; } = string.Empty;
    public string? TourPlanId { get; set; }
    public string? TicketReference { get; set; }
}

public class ForexMainDto
{
    public string Id { get; set; } = string.Empty;
    public string TourPlanId { get; set; } = string.Empty;
    public string PassportNo { get; set; } = string.Empty;
    public string PassportName { get; set; } = string.Empty;
    public DateTime PassportExpiryDate { get; set; }
    public string? Status { get; set; }
    public string Currency { get; set; } = string.Empty;
    public decimal TotalValue { get; set; }
    public string RequestType { get; set; } = string.Empty;
}

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
