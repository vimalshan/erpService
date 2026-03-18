using SettlementService.Domain.Common;
using SettlementService.Domain.Entities;
using SettlementService.Domain.Enums;
using SettlementService.Domain.Events;

namespace SettlementService.Domain.Aggregates;

public class Settlement : BaseEntity
{
    public long StSetNum { get; private set; }
    public string? StTrustCode { get; private set; }
    public long? StMemberNo { get; private set; }
    public string? StSetType { get; private set; }
    public DateTime? StSetDate { get; private set; }
    public DateTime? StDolDat { get; private set; }
    public string? StReason { get; private set; }
    public DateTime? StUpdOn { get; private set; }
    public long? StUpdByEmpSysId { get; private set; }
    public DateTime? StAccDate { get; private set; }
    public long? StFinYear { get; private set; }
    public string? StJvVoucherType { get; private set; }
    public long? StJvNo { get; private set; }
    public string? StSetIntFlg { get; private set; }
    public string? StTaxSts { get; private set; }
    public long? StTaxRate { get; private set; }
    public decimal? StSettlementAmount { get; private set; }
    public SettlementStatus StStatus { get; private set; }

    private readonly List<SettlementDeduction> _deductions = [];
    private readonly List<SettlementApproval> _approvals = [];
    private readonly List<SettlementPayment> _payments = [];

    public IReadOnlyCollection<SettlementDeduction> Deductions => _deductions.AsReadOnly();
    public IReadOnlyCollection<SettlementApproval> Approvals => _approvals.AsReadOnly();
    public IReadOnlyCollection<SettlementPayment> Payments => _payments.AsReadOnly();

    private Settlement() { }

    public Settlement(
        long setNum,
        long memberNo,
        string setType,
        decimal settlementAmount,
        DateTime settlementDate,
        long createdBy,
        string? trustCode = null,
        string? reason = null)
    {
        StSetNum = setNum;
        StMemberNo = memberNo;
        StSetType = setType;
        StSettlementAmount = settlementAmount;
        StSetDate = settlementDate;
        StUpdByEmpSysId = createdBy;
        StUpdOn = DateTime.UtcNow;
        StTrustCode = trustCode;
        StReason = reason;
        StStatus = SettlementStatus.Pending;

        AddDomainEvent(new SettlementCreatedEvent(StSetNum, memberNo, settlementAmount));
    }

    public void Approve(long approvedBy, string? remarks = null)
    {
        if (StStatus != SettlementStatus.Pending)
            throw new InvalidOperationException("Only pending settlements can be approved.");

        StStatus = SettlementStatus.Approved;
        StUpdOn = DateTime.UtcNow;
        StUpdByEmpSysId = approvedBy;

        var approval = new SettlementApproval(StSetNum, _approvals.Count + 1, approvedBy, remarks);
        approval.Approve(remarks);
        _approvals.Add(approval);

        AddDomainEvent(new SettlementApprovedEvent(StSetNum, approvedBy));
    }

    public void Reject(long rejectedBy, string? remarks = null)
    {
        if (StStatus != SettlementStatus.Pending)
            throw new InvalidOperationException("Only pending settlements can be rejected.");

        StStatus = SettlementStatus.Rejected;
        StUpdOn = DateTime.UtcNow;
        StUpdByEmpSysId = rejectedBy;

        var approval = new SettlementApproval(StSetNum, _approvals.Count + 1, rejectedBy, remarks);
        approval.Reject(remarks);
        _approvals.Add(approval);

        AddDomainEvent(new SettlementRejectedEvent(StSetNum, rejectedBy, remarks));
    }

    public void Complete()
    {
        if (StStatus != SettlementStatus.Approved)
            throw new InvalidOperationException("Only approved settlements can be completed.");

        StStatus = SettlementStatus.Completed;
        StUpdOn = DateTime.UtcNow;

        AddDomainEvent(new SettlementCompletedEvent(StSetNum));
    }

    public void AddDeduction(string dedType, decimal dedAmount)
    {
        var deduction = new SettlementDeduction(StSetNum, dedType, dedAmount);
        _deductions.Add(deduction);
    }

    public void AddPayment(string payMode, decimal payAmount, string? payRefNo = null)
    {
        if (StStatus != SettlementStatus.Approved)
            throw new InvalidOperationException("Payments can only be added to approved settlements.");

        var payment = new SettlementPayment(StSetNum, payMode, payAmount, payRefNo);
        _payments.Add(payment);

        AddDomainEvent(new SettlementPaymentAddedEvent(StSetNum, payAmount, payMode));
    }

    public void UpdateSettlementDetails(
        string? reason = null,
        string? taxStatus = null,
        long? taxRate = null,
        string? setIntFlg = null)
    {
        if (StStatus != SettlementStatus.Pending)
            throw new InvalidOperationException("Only pending settlements can be updated.");

        if (reason != null) StReason = reason;
        if (taxStatus != null) StTaxSts = taxStatus;
        if (taxRate.HasValue) StTaxRate = taxRate;
        if (setIntFlg != null) StSetIntFlg = setIntFlg;
        StUpdOn = DateTime.UtcNow;
    }
}
