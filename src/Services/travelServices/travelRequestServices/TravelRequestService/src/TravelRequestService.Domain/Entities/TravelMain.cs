using TravelRequestService.Domain.Common;
using TravelRequestService.Domain.Enums;
using TravelRequestService.Domain.Events;
using TravelRequestService.Domain.ValueObjects;

namespace TravelRequestService.Domain.Entities;

public class TravelMain : AggregateRoot
{
    public string CompanyCode { get; private set; } = null!;
    public long PlanNumber { get; private set; }
    public string? UserCode { get; private set; }
    public long? UserNumber { get; private set; }
    public DateTime? AppliedDate { get; private set; }
    public DateTime? ModifiedDate { get; private set; }
    public string? ModifiedBy { get; private set; }
    public long? NatureCode { get; private set; }
    public string? ObjectiveDescription { get; private set; }
    public string? Remarks { get; private set; }
    public string? TripOutcome { get; private set; }
    public bool IsBudgeted { get; private set; }
    public TravelRequestStatus Status { get; private set; }
    public SettlementStatus? SettlementStatus { get; private set; }
    public bool? TripFlag { get; private set; }
    public decimal? BudgetAmount { get; private set; }
    public decimal? ActualAmount { get; private set; }
    public decimal? AdvanceAmount { get; private set; }
    public decimal? PaidAmount { get; private set; }
    public decimal? AdjustedAmount { get; private set; }
    public decimal? RequestId { get; private set; }
    public TravelType TravelType { get; private set; }
    public bool CurrencyPreference { get; private set; }
    public decimal? AdditionalAmount { get; private set; }
    public bool? SpecialSanction { get; private set; }
    public long? FinancialUnit { get; private set; }
    public string? CcrRemarks { get; private set; }
    public bool? BypassApproval { get; private set; }
    public bool? AccountTender { get; private set; }
    public string? BypassRemarks { get; private set; }

    // Navigation properties
    private readonly List<TravelSub> _subDetails = [];
    public IReadOnlyCollection<TravelSub> SubDetails => _subDetails.AsReadOnly();

    private readonly List<TravelAgenda> _agendas = [];
    public IReadOnlyCollection<TravelAgenda> Agendas => _agendas.AsReadOnly();

    private readonly List<TravelAdvance> _advances = [];
    public IReadOnlyCollection<TravelAdvance> Advances => _advances.AsReadOnly();

    private readonly List<TravelApprovalRemark> _approvalRemarks = [];
    public IReadOnlyCollection<TravelApprovalRemark> ApprovalRemarks => _approvalRemarks.AsReadOnly();

    private TravelMain() { }

    public static TravelMain Create(
        string companyCode,
        long planNumber,
        long userNumber,
        string? objective,
        TravelType travelType,
        decimal? budgetAmount)
    {
        var travel = new TravelMain
        {
            CompanyCode = companyCode,
            PlanNumber = planNumber,
            UserNumber = userNumber,
            AppliedDate = DateTime.UtcNow,
            ObjectiveDescription = objective,
            Status = TravelRequestStatus.Pending,
            TravelType = travelType,
            BudgetAmount = budgetAmount,
            IsBudgeted = budgetAmount.HasValue && budgetAmount.Value > 0
        };

        travel.AddDomainEvent(new TravelRequestCreatedEvent(travel.PlanNumber, travel.CompanyCode));
        return travel;
    }

    public void Approve(long approvedBy, decimal approvalAmount, string? remarks)
    {
        if (Status != TravelRequestStatus.Pending)
            throw new InvalidOperationException("Only pending requests can be approved.");

        Status = TravelRequestStatus.Approved;
        BudgetAmount = approvalAmount;
        ModifiedDate = DateTime.UtcNow;
        ModifiedBy = approvedBy.ToString();

        if (!string.IsNullOrWhiteSpace(remarks))
            _approvalRemarks.Add(TravelApprovalRemark.Create(PlanNumber, "APPROVAL", remarks, approvedBy.ToString()));

        AddDomainEvent(new TravelRequestApprovedEvent(PlanNumber, CompanyCode, approvedBy));
    }

    public void Reject(long rejectedBy, string? remarks)
    {
        if (Status != TravelRequestStatus.Pending)
            throw new InvalidOperationException("Only pending requests can be rejected.");

        Status = TravelRequestStatus.Rejected;
        ModifiedDate = DateTime.UtcNow;
        ModifiedBy = rejectedBy.ToString();

        if (!string.IsNullOrWhiteSpace(remarks))
            _approvalRemarks.Add(TravelApprovalRemark.Create(PlanNumber, "REJECTION", remarks, rejectedBy.ToString()));

        AddDomainEvent(new TravelRequestRejectedEvent(PlanNumber, CompanyCode, rejectedBy));
    }

    public void Cancel(string? cancelRemarks)
    {
        Status = TravelRequestStatus.Cancelled;
        ModifiedDate = DateTime.UtcNow;
        Remarks = cancelRemarks;
        AddDomainEvent(new TravelRequestCancelledEvent(PlanNumber, CompanyCode));
    }

    public void AddAgenda(TravelAgenda agenda) => _agendas.Add(agenda);
    public void AddSubDetail(TravelSub sub) => _subDetails.Add(sub);

    public void AddAdvance(TravelAdvance advance)
    {
        if (BudgetAmount.HasValue)
        {
            var totalAdvances = _advances.Sum(a => a.AdvanceAmount ?? 0) + (advance.AdvanceAmount ?? 0);
            if (totalAdvances > BudgetAmount.Value)
                throw new InvalidOperationException("Advance amount exceeds budget allocation.");
        }
        _advances.Add(advance);
    }

    public void UpdateSettlementStatus(SettlementStatus status)
    {
        SettlementStatus = status;
        ModifiedDate = DateTime.UtcNow;
    }
}
