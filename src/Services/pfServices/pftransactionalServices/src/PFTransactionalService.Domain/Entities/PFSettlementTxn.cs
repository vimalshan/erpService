using PFTransactionalService.Domain.Common;
using PFTransactionalService.Domain.Enums;

namespace PFTransactionalService.Domain.Entities;

public class PFSettlementTxn : BaseEntity
{
    public long PfSettlementTxnId { get; private set; }
    public long PfSettlementId { get; private set; }
    public long EmpSysId { get; private set; }
    public decimal PfSettlementTxnAmount { get; private set; }
    public DateTime PfSettlementTxnDate { get; private set; }
    public TransactionStatus PfSettlementTxnStatus { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }

    private PFSettlementTxn() { }

    public PFSettlementTxn(long settlementId, long empSysId, decimal amount, long createdBy)
    {
        PfSettlementId = settlementId;
        EmpSysId = empSysId;
        PfSettlementTxnAmount = amount;
        PfSettlementTxnDate = DateTime.UtcNow;
        PfSettlementTxnStatus = TransactionStatus.Posted;
        CreatedBy = createdBy;
        CreatedOn = DateTime.UtcNow;
    }

    public void Reverse()
    {
        if (PfSettlementTxnStatus != TransactionStatus.Posted)
            throw new InvalidOperationException("Only posted transactions can be reversed.");
        PfSettlementTxnStatus = TransactionStatus.Reversed;
    }
}
