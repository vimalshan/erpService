using TourPlanService.Domain.Common;
using TourPlanService.Domain.Enums;
using TourPlanService.Domain.Events;
using TourPlanService.Domain.Exceptions;

namespace TourPlanService.Domain.Entities;

/// <summary>TOURPLAN_MAIN - Tour Plan Aggregate Root</summary>
public sealed class TourPlan : BaseEntity
{
    private readonly List<TourAdvance> _advances = [];
    private readonly List<TourAgenda> _agendas = [];
    private readonly List<TourCostCentre> _costCentres = [];
    private readonly List<TourDaBreak> _daBreaks = [];
    private readonly List<TourExpense> _expenses = [];
    private readonly List<InternationalSchedule> _intSchedules = [];
    private readonly List<TourLeave> _leaves = [];
    private readonly List<NmsSchedule> _nmsSchedules = [];
    private readonly List<SelfExpense> _selfExpenses = [];
    private readonly List<ForexRequisition> _forexRequisitions = [];
    private readonly List<DomesticDaBreak> _domesticDaBreaks = [];
    private readonly List<ForeignExpenseMain> _foreignExpenses = [];

    // Private constructor for EF Core
    private TourPlan() { }

    public string TpId { get; private set; } = default!;
    public string TpEmpSysId { get; private set; } = default!;
    public DateTime TpStartDate { get; private set; }
    public DateTime? TpEndDate { get; private set; }
    public string TpPurpose { get; private set; } = default!;
    public string TpRemarks { get; private set; } = default!;
    public string TpStatus { get; private set; } = default!;
    public string TpCategory { get; private set; } = default!;
    public string TpBookInc { get; private set; } = default!;
    public string? TpType { get; private set; }
    public string TpCreatedBy { get; private set; } = default!;
    public DateTime TpCreatedOn { get; private set; }
    public string? TpApprovedBy { get; private set; }
    public DateTime? TpApprovedOn { get; private set; }
    public string TpLastModifiedBy { get; private set; } = default!;
    public DateTime TpLastModifiedOn { get; private set; }
    public string TpFromCityId { get; private set; } = default!;
    public string TpFromCityName { get; private set; } = default!;
    public string TpToCityId { get; private set; } = default!;
    public string TpToCityName { get; private set; } = default!;
    public string TpSupRemarks { get; private set; } = default!;
    public string? TpContactNo { get; private set; }
    public string? TpGradeType { get; private set; }
    public string? TpHomeCountryId { get; private set; }
    public string? TpTravelSectorId { get; private set; }
    public string? TpCostEffective { get; private set; }
    public string? TpCostJustify { get; private set; }
    public string? TpClaimType { get; private set; }
    public string? TpSpecialRemarks { get; private set; }
    public string? TpAppRemarks { get; private set; }
    public string? TpAppLevel { get; private set; }
    public string? TpBalPayAmt { get; private set; }
    public string? TpCeoEmpSysId { get; private set; }
    public DateTime? TpDaEffDate { get; private set; }
    public DateTime? TpDaClsDate { get; private set; }
    public string? TpDaValue { get; private set; }
    public string? TpDaToolTip { get; private set; }
    public string? TpExpStatus { get; private set; }
    public string? TpExpApprovedBy { get; private set; }
    public DateTime? TpExpApprovedOn { get; private set; }
    public string? TpRecommenderSysId { get; private set; }
    public string? TpPayUnitId { get; private set; }
    public string? TpDaDays { get; private set; }
    public string? TpDaRate { get; private set; }
    public string? TpExpPayMode { get; private set; }
    public string? TpExpJvId { get; private set; }
    public DateTime? TpExpSubmitedOn { get; private set; }
    public string? TpExpSubmitedBy { get; private set; }
    public string? TpEstimateConvRate1 { get; private set; }
    public string? TpEstimateConvRate2 { get; private set; }
    public string? TpActRemarks { get; private set; }
    public string? TpEstimateConvRate3 { get; private set; }
    public string? TpClosureStatus { get; private set; }

    // Navigation properties
    public IReadOnlyCollection<TourAdvance> Advances => _advances.AsReadOnly();
    public IReadOnlyCollection<TourAgenda> Agendas => _agendas.AsReadOnly();
    public IReadOnlyCollection<TourCostCentre> CostCentres => _costCentres.AsReadOnly();
    public IReadOnlyCollection<TourDaBreak> DaBreaks => _daBreaks.AsReadOnly();
    public IReadOnlyCollection<TourExpense> Expenses => _expenses.AsReadOnly();
    public IReadOnlyCollection<InternationalSchedule> IntSchedules => _intSchedules.AsReadOnly();
    public IReadOnlyCollection<TourLeave> Leaves => _leaves.AsReadOnly();
    public IReadOnlyCollection<NmsSchedule> NmsSchedules => _nmsSchedules.AsReadOnly();
    public IReadOnlyCollection<SelfExpense> SelfExpenses => _selfExpenses.AsReadOnly();
    public IReadOnlyCollection<ForexRequisition> ForexRequisitions => _forexRequisitions.AsReadOnly();
    public IReadOnlyCollection<DomesticDaBreak> DomesticDaBreaks => _domesticDaBreaks.AsReadOnly();
    public IReadOnlyCollection<ForeignExpenseMain> ForeignExpenses => _foreignExpenses.AsReadOnly();

    public static TourPlan Create(
        string tpId,
        string empSysId,
        DateTime startDate,
        string purpose,
        string remarks,
        string category,
        string bookInc,
        string fromCityId,
        string fromCityName,
        string toCityId,
        string toCityName,
        string supRemarks,
        string createdBy)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tpId);
        ArgumentException.ThrowIfNullOrWhiteSpace(empSysId);

        var tourPlan = new TourPlan
        {
            TpId = tpId,
            TpEmpSysId = empSysId,
            TpStartDate = startDate,
            TpPurpose = purpose,
            TpRemarks = remarks,
            TpStatus = "DRAFT",
            TpCategory = category,
            TpBookInc = bookInc,
            TpFromCityId = fromCityId,
            TpFromCityName = fromCityName,
            TpToCityId = toCityId,
            TpToCityName = toCityName,
            TpSupRemarks = supRemarks,
            TpCreatedBy = createdBy,
            TpCreatedOn = DateTime.UtcNow,
            TpLastModifiedBy = createdBy,
            TpLastModifiedOn = DateTime.UtcNow
        };

        tourPlan.RaiseDomainEvent(new TourPlanCreatedEvent(
            Guid.NewGuid(), tpId, empSysId, createdBy, DateTime.UtcNow));

        return tourPlan;
    }

    public void Approve(string approvedBy, string? remarks = null)
    {
        if (TpStatus == "APPROVED")
            throw new DomainException("Tour plan is already approved.");

        var oldStatus = TpStatus;
        TpStatus = "APPROVED";
        TpApprovedBy = approvedBy;
        TpApprovedOn = DateTime.UtcNow;
        TpAppRemarks = remarks;
        TpLastModifiedBy = approvedBy;
        TpLastModifiedOn = DateTime.UtcNow;

        RaiseDomainEvent(new TourPlanApprovedEvent(Guid.NewGuid(), TpId, approvedBy, DateTime.UtcNow));
        RaiseDomainEvent(new TourPlanStatusChangedEvent(Guid.NewGuid(), TpId, oldStatus, TpStatus, approvedBy, DateTime.UtcNow));
    }

    public void Reject(string rejectedBy, string remarks)
    {
        var oldStatus = TpStatus;
        TpStatus = "REJECTED";
        TpAppRemarks = remarks;
        TpLastModifiedBy = rejectedBy;
        TpLastModifiedOn = DateTime.UtcNow;

        RaiseDomainEvent(new TourPlanStatusChangedEvent(Guid.NewGuid(), TpId, oldStatus, TpStatus, rejectedBy, DateTime.UtcNow));
    }

    public void SubmitExpense(string submittedBy)
    {
        if (TpStatus != "APPROVED")
            throw new DomainException("Only approved tour plans can have expense submitted.");

        TpExpStatus = "SUBMITTED";
        TpExpSubmitedBy = submittedBy;
        TpExpSubmitedOn = DateTime.UtcNow;
        TpLastModifiedBy = submittedBy;
        TpLastModifiedOn = DateTime.UtcNow;

        RaiseDomainEvent(new TourPlanExpenseSubmittedEvent(Guid.NewGuid(), TpId, submittedBy, DateTime.UtcNow));
    }

    public void UpdateStatus(string newStatus, string modifiedBy)
    {
        var oldStatus = TpStatus;
        TpStatus = newStatus;
        TpLastModifiedBy = modifiedBy;
        TpLastModifiedOn = DateTime.UtcNow;

        RaiseDomainEvent(new TourPlanStatusChangedEvent(Guid.NewGuid(), TpId, oldStatus, newStatus, modifiedBy, DateTime.UtcNow));
    }

    public void AddAdvance(TourAdvance advance) => _advances.Add(advance);
    public void AddAgenda(TourAgenda agenda) => _agendas.Add(agenda);
    public void AddCostCentre(TourCostCentre costCentre) => _costCentres.Add(costCentre);
    public void AddDaBreak(TourDaBreak daBreak) => _daBreaks.Add(daBreak);
    public void AddExpense(TourExpense expense) => _expenses.Add(expense);
    public void AddIntSchedule(InternationalSchedule schedule) => _intSchedules.Add(schedule);
    public void AddLeave(TourLeave leave) => _leaves.Add(leave);
    public void AddNmsSchedule(NmsSchedule schedule) => _nmsSchedules.Add(schedule);
    public void AddSelfExpense(SelfExpense selfExp) => _selfExpenses.Add(selfExp);
    public void AddForexRequisition(ForexRequisition forex) => _forexRequisitions.Add(forex);
    public void AddDomesticDaBreak(DomesticDaBreak daBreak) => _domesticDaBreaks.Add(daBreak);
    public void AddForeignExpense(ForeignExpenseMain expense) => _foreignExpenses.Add(expense);
}
