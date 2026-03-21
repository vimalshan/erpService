using TravelService.Domain.Common;
using TravelService.Domain.Events;
using TravelService.Domain.ValueObjects;

namespace TravelService.Domain.Entities.TourPlan;

public class TourPlan : AggregateRoot<string>
{
    public string EmployeeSysId { get; private set; } = string.Empty;
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string Purpose { get; private set; } = string.Empty;
    public string Remarks { get; private set; } = string.Empty;
    public string Status { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;
    public bool IncludeBookingRequests { get; private set; }
    public string? TripType { get; private set; }
    public string CreatedBy { get; private set; } = string.Empty;
    public DateTime CreatedOn { get; private set; }
    public string? ApprovedBy { get; private set; }
    public DateTime? ApprovedOn { get; private set; }
    public string LastModifiedBy { get; private set; } = string.Empty;
    public DateTime LastModifiedOn { get; private set; }
    public CityInfo FromCity { get; private set; } = null!;
    public CityInfo ToCity { get; private set; } = null!;
    public string SupervisorRemarks { get; private set; } = string.Empty;
    public string? ContactNo { get; private set; }
    public string? GradeType { get; private set; }
    public string? PayrollUnitId { get; private set; }
    public string? ClaimType { get; private set; }
    public string? ApproverRemarks { get; private set; }
    public string? ExpenseStatus { get; private set; }
    public string? ClosureStatus { get; private set; }
    public string? ActualOutcome { get; private set; }

    private readonly List<TourPlanAdvance> _advances = new();
    private readonly List<TourPlanAgenda> _agendas = new();
    private readonly List<TourPlanCostCentre> _costCentres = new();
    private readonly List<TourPlanDaBreak> _daBreaks = new();
    private readonly List<TourPlanExpense> _expenses = new();
    private readonly List<TourPlanIntSchedule> _intSchedules = new();
    private readonly List<TourPlanLeave> _leaves = new();
    private readonly List<TourPlanNmsSchedule> _nmsSchedules = new();
    private readonly List<TourPlanSelfExpense> _selfExpenses = new();

    public IReadOnlyCollection<TourPlanAdvance> Advances => _advances.AsReadOnly();
    public IReadOnlyCollection<TourPlanAgenda> Agendas => _agendas.AsReadOnly();
    public IReadOnlyCollection<TourPlanCostCentre> CostCentres => _costCentres.AsReadOnly();
    public IReadOnlyCollection<TourPlanDaBreak> DaBreaks => _daBreaks.AsReadOnly();
    public IReadOnlyCollection<TourPlanExpense> Expenses => _expenses.AsReadOnly();
    public IReadOnlyCollection<TourPlanIntSchedule> IntSchedules => _intSchedules.AsReadOnly();
    public IReadOnlyCollection<TourPlanLeave> Leaves => _leaves.AsReadOnly();
    public IReadOnlyCollection<TourPlanNmsSchedule> NmsSchedules => _nmsSchedules.AsReadOnly();
    public IReadOnlyCollection<TourPlanSelfExpense> SelfExpenses => _selfExpenses.AsReadOnly();

    protected TourPlan() { }

    public static TourPlan Create(
        string id, string employeeSysId, DateTime startDate, DateTime? endDate,
        string purpose, string remarks, string category, bool includeBooking,
        CityInfo fromCity, CityInfo toCity, string supervisorRemarks,
        string createdBy, string payrollUnitId, string? tripType = null,
        string? gradeType = null, string? contactNo = null)
    {
        var tp = new TourPlan
        {
            Id = id,
            EmployeeSysId = employeeSysId,
            StartDate = startDate,
            EndDate = endDate,
            Purpose = purpose,
            Remarks = remarks,
            Category = category,
            IncludeBookingRequests = includeBooking,
            FromCity = fromCity,
            ToCity = toCity,
            SupervisorRemarks = supervisorRemarks,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow,
            LastModifiedBy = createdBy,
            LastModifiedOn = DateTime.UtcNow,
            Status = "DRAFT",
            TripType = tripType,
            GradeType = gradeType,
            ContactNo = contactNo,
            PayrollUnitId = payrollUnitId
        };
        tp.RaiseDomainEvent(new TourPlanCreatedEvent(id, employeeSysId));
        return tp;
    }

    public void Submit(string modifiedBy)
    {
        if (Status != "DRAFT")
            throw new InvalidOperationException("Only draft tour plans can be submitted.");
        Status = "SUBMITTED";
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
        RaiseDomainEvent(new TourPlanSubmittedEvent(Id, EmployeeSysId));
    }

    public void Approve(string approvedBy, string? remarks = null)
    {
        if (Status != "SUBMITTED" && Status != "PENDING")
            throw new InvalidOperationException("Tour plan is not in a state to be approved.");
        Status = "APPROVED";
        ApprovedBy = approvedBy;
        ApprovedOn = DateTime.UtcNow;
        ApproverRemarks = remarks;
        LastModifiedBy = approvedBy;
        LastModifiedOn = DateTime.UtcNow;
        RaiseDomainEvent(new TourPlanApprovedEvent(Id, EmployeeSysId, approvedBy));
    }

    public void Reject(string rejectedBy, string remarks)
    {
        if (Status != "SUBMITTED" && Status != "PENDING")
            throw new InvalidOperationException("Tour plan is not in a state to be rejected.");
        Status = "REJECTED";
        ApproverRemarks = remarks;
        LastModifiedBy = rejectedBy;
        LastModifiedOn = DateTime.UtcNow;
        RaiseDomainEvent(new TourPlanRejectedEvent(Id, EmployeeSysId, rejectedBy, remarks));
    }

    public void Cancel(string cancelledBy)
    {
        if (Status == "CLOSED" || Status == "CANCELLED")
            throw new InvalidOperationException("Tour plan is already closed or cancelled.");
        Status = "CANCELLED";
        LastModifiedBy = cancelledBy;
        LastModifiedOn = DateTime.UtcNow;
    }

    public void Close(string closedBy, string actualOutcome)
    {
        if (Status != "APPROVED")
            throw new InvalidOperationException("Only approved tour plans can be closed.");
        Status = "CLOSED";
        ClosureStatus = "C";
        ActualOutcome = actualOutcome;
        LastModifiedBy = closedBy;
        LastModifiedOn = DateTime.UtcNow;
    }

    public void AddAdvance(TourPlanAdvance advance) => _advances.Add(advance);
    public void AddAgenda(TourPlanAgenda agenda) => _agendas.Add(agenda);
    public void AddCostCentre(TourPlanCostCentre costCentre) => _costCentres.Add(costCentre);
    public void AddDaBreak(TourPlanDaBreak daBreak) => _daBreaks.Add(daBreak);
    public void AddExpense(TourPlanExpense expense) => _expenses.Add(expense);
    public void AddIntSchedule(TourPlanIntSchedule schedule) => _intSchedules.Add(schedule);
    public void AddLeave(TourPlanLeave leave) => _leaves.Add(leave);
    public void AddNmsSchedule(TourPlanNmsSchedule schedule) => _nmsSchedules.Add(schedule);
    public void AddSelfExpense(TourPlanSelfExpense expense) => _selfExpenses.Add(expense);
}
