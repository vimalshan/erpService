using PFTransactionalService.Domain.Common;
using PFTransactionalService.Domain.Enums;

namespace PFTransactionalService.Domain.Entities;

/// <summary>
/// PF Contribution Transaction - records individual monthly contribution.
/// </summary>
public class PFContributionTxn : BaseEntity
{
    public long PfTxnId { get; private set; }
    public long EmpSysId { get; private set; }
    public decimal PfEmpContribution { get; private set; }
    public decimal PfErContribution { get; private set; }
    public decimal PfVolContribution { get; private set; }
    public DateTime PfTxnDate { get; private set; }
    public DateTime PfTxnMonth { get; private set; }
    public TransactionStatus PfTxnStatus { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }

    private PFContributionTxn() { }

    public PFContributionTxn(
        long empSysId,
        decimal empContribution,
        decimal erContribution,
        decimal volContribution,
        DateTime txnDate,
        DateTime txnMonth,
        long createdBy)
    {
        EmpSysId = empSysId;
        PfEmpContribution = empContribution;
        PfErContribution = erContribution;
        PfVolContribution = volContribution;
        PfTxnDate = txnDate;
        PfTxnMonth = txnMonth;
        PfTxnStatus = TransactionStatus.Posted;
        CreatedBy = createdBy;
        CreatedOn = DateTime.UtcNow;
    }

    public void Reverse()
    {
        if (PfTxnStatus != TransactionStatus.Posted)
            throw new InvalidOperationException("Only posted transactions can be reversed.");
        PfTxnStatus = TransactionStatus.Reversed;
    }

    public void Cancel()
    {
        if (PfTxnStatus != TransactionStatus.Pending)
            throw new InvalidOperationException("Only pending transactions can be cancelled.");
        PfTxnStatus = TransactionStatus.Cancelled;
    }
}
