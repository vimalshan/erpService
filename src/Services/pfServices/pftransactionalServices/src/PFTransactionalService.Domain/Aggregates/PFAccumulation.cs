using PFTransactionalService.Domain.Common;
using PFTransactionalService.Domain.Entities;
using PFTransactionalService.Domain.Enums;
using PFTransactionalService.Domain.Events;

namespace PFTransactionalService.Domain.Aggregates;

/// <summary>
/// Aggregate root: PF Accumulation per member - tracks running PF balance.
/// </summary>
public class PFAccumulation : BaseEntity
{
    public long PfAccId { get; private set; }
    public long EmpSysId { get; private set; }
    public long MemberNo { get; private set; }
    public string TrustCode { get; private set; } = string.Empty;
    public decimal PfAccBal { get; private set; }
    public decimal PfEmpContTotal { get; private set; }
    public decimal PfErContTotal { get; private set; }
    public decimal PfVolContTotal { get; private set; }
    public AccumulationStatus PfAccStatus { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long? UpdatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }

    private readonly List<PFContributionTxn> _contributions = [];
    private readonly List<PFWithdrawalCertificate> _certificates = [];

    public IReadOnlyCollection<PFContributionTxn> Contributions => _contributions.AsReadOnly();
    public IReadOnlyCollection<PFWithdrawalCertificate> Certificates => _certificates.AsReadOnly();

    private PFAccumulation() { }

    public PFAccumulation(
        long empSysId,
        long memberNo,
        string trustCode,
        decimal initialBalance,
        decimal empContribution,
        decimal erContribution,
        long createdBy)
    {
        EmpSysId = empSysId;
        MemberNo = memberNo;
        TrustCode = trustCode;
        PfAccBal = initialBalance;
        PfEmpContTotal = empContribution;
        PfErContTotal = erContribution;
        PfVolContTotal = 0;
        PfAccStatus = AccumulationStatus.Active;
        CreatedBy = createdBy;
        CreatedOn = DateTime.UtcNow;

        AddDomainEvent(new PFAccumulationCreatedEvent(EmpSysId, MemberNo, TrustCode, initialBalance));
    }

    public void AddContribution(decimal empContribution, decimal erContribution, decimal volContribution, DateTime txnMonth, long processedBy)
    {
        if (PfAccStatus != AccumulationStatus.Active)
            throw new InvalidOperationException("Cannot add contribution to a non-active accumulation.");

        var totalContribution = empContribution + erContribution + volContribution;
        PfAccBal += totalContribution;
        PfEmpContTotal += empContribution;
        PfErContTotal += erContribution;
        PfVolContTotal += volContribution;
        UpdatedBy = processedBy;
        UpdatedOn = DateTime.UtcNow;

        var txn = new PFContributionTxn(
            EmpSysId, empContribution, erContribution, volContribution,
            DateTime.UtcNow, txnMonth, processedBy);
        _contributions.Add(txn);

        AddDomainEvent(new ContributionPostedEvent(EmpSysId, MemberNo, empContribution, erContribution, txnMonth));
    }

    public void ProcessWithdrawal(decimal amount, long processedBy)
    {
        if (PfAccStatus != AccumulationStatus.Active)
            throw new InvalidOperationException("Cannot withdraw from a non-active accumulation.");

        if (amount > PfAccBal)
            throw new InvalidOperationException("Withdrawal amount exceeds accumulated PF balance.");

        PfAccBal -= amount;
        UpdatedBy = processedBy;
        UpdatedOn = DateTime.UtcNow;

        AddDomainEvent(new WithdrawalProcessedEvent(EmpSysId, MemberNo, amount));
    }

    public void ApplyInterest(decimal interestAmount, long processedBy)
    {
        if (PfAccStatus != AccumulationStatus.Active)
            throw new InvalidOperationException("Cannot apply interest to a non-active accumulation.");

        PfAccBal += interestAmount;
        UpdatedBy = processedBy;
        UpdatedOn = DateTime.UtcNow;

        AddDomainEvent(new InterestAppliedEvent(EmpSysId, MemberNo, interestAmount));
    }

    public void Close(long closedBy)
    {
        if (PfAccStatus != AccumulationStatus.Active)
            throw new InvalidOperationException("Only active accumulations can be closed.");

        PfAccStatus = AccumulationStatus.Closed;
        UpdatedBy = closedBy;
        UpdatedOn = DateTime.UtcNow;

        AddDomainEvent(new PFAccumulationClosedEvent(EmpSysId, MemberNo));
    }

    public void Freeze(long frozenBy)
    {
        if (PfAccStatus != AccumulationStatus.Active)
            throw new InvalidOperationException("Only active accumulations can be frozen.");

        PfAccStatus = AccumulationStatus.Frozen;
        UpdatedBy = frozenBy;
        UpdatedOn = DateTime.UtcNow;
    }

    public void Reactivate(long reactivatedBy)
    {
        if (PfAccStatus != AccumulationStatus.Frozen)
            throw new InvalidOperationException("Only frozen accumulations can be reactivated.");

        PfAccStatus = AccumulationStatus.Active;
        UpdatedBy = reactivatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
