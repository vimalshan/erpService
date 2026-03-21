using TravelRequestService.Domain.Enums;

namespace TravelRequestService.Application.DTOs;

public record TravelRequestDto
{
    public long PlanNumber { get; init; }
    public string CompanyCode { get; init; } = null!;
    public long? UserNumber { get; init; }
    public string? UserCode { get; init; }
    public DateTime? AppliedDate { get; init; }
    public string? ObjectiveDescription { get; init; }
    public string? Remarks { get; init; }
    public string? TripOutcome { get; init; }
    public bool IsBudgeted { get; init; }
    public string Status { get; init; } = null!;
    public string TravelType { get; init; } = null!;
    public decimal? BudgetAmount { get; init; }
    public decimal? ActualAmount { get; init; }
    public decimal? AdvanceAmount { get; init; }
    public List<TravelSubDto> SubDetails { get; init; } = [];
    public List<TravelAgendaDto> Agendas { get; init; } = [];
    public List<TravelAdvanceDto> Advances { get; init; } = [];
    public List<TravelApprovalRemarkDto> ApprovalRemarks { get; init; } = [];
}

public record TravelSubDto
{
    public long RequestNumber { get; init; }
    public long SerialNumber { get; init; }
    public long? BookingNumber { get; init; }
    public DateTime? CancelDate { get; init; }
    public string? CancelRemarks { get; init; }
    public bool OnDuty { get; init; }
}

public record TravelAgendaDto
{
    public long RequestNumber { get; init; }
    public int SerialNumber { get; init; }
    public DateTime? MeetingDate { get; init; }
    public string? PeopleToMeet { get; init; }
    public string? DesiredOutcome { get; init; }
    public string? CityName { get; init; }
}

public record TravelAdvanceDto
{
    public long RequestNumber { get; init; }
    public long AdvanceNumber { get; init; }
    public DateTime? AdvanceDate { get; init; }
    public decimal? AdvanceAmount { get; init; }
    public decimal? ApprovedAmount { get; init; }
    public decimal? PaidAmount { get; init; }
    public DateTime? PaidDate { get; init; }
    public string? PayType { get; init; }
}

public record TravelApprovalRemarkDto
{
    public long RequestNumber { get; init; }
    public string? RequestType { get; init; }
    public string? Remarks { get; init; }
    public string? ApprovedBy { get; init; }
    public DateTime? ApprovedOn { get; init; }
}

public record DashTourPlanDto
{
    public DateTime? TourDate { get; init; }
    public string? Business { get; init; }
    public string? Unit { get; init; }
    public long? EmployeeSystemId { get; init; }
    public string? EmployeeName { get; init; }
    public string? Grade { get; init; }
    public long? TourNumber { get; init; }
    public decimal? ExpenseAmount { get; init; }
    public string? Nature { get; init; }
}
