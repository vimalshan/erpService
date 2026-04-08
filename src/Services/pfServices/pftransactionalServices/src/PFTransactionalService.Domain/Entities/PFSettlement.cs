using PFTransactionalService.Domain.Common;
using PFTransactionalService.Domain.Enums;

namespace PFTransactionalService.Domain.Entities;

public class PFSettlement : BaseEntity
{
    public long PfSettlementId { get; private set; }
    public long EmpSysId { get; private set; }
    public decimal PfSettlementAmount { get; private set; }
    public string PfSettlementType { get; private set; } = string.Empty;
    public DateTime PfSettlementDate { get; private set; }
    public TransactionStatus PfSettlementStatus { get; private set; }
    public long ApprovedBy { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }

    private readonly List<PFSettlementTxn> _transactions = [];
    public IReadOnlyCollection<PFSettlementTxn> Transactions => _transactions.AsReadOnly();

    private PFSettlement() { }

    public PFSettlement(long empSysId, decimal amount, string settlementType, long approvedBy, long createdBy)
    {
        EmpSysId = empSysId;
        PfSettlementAmount = amount;
        PfSettlementType = settlementType;
        PfSettlementDate = DateTime.UtcNow;
        PfSettlementStatus = TransactionStatus.Posted;
        ApprovedBy = approvedBy;
        CreatedBy = createdBy;
        CreatedOn = DateTime.UtcNow;
    }

    public void AddTransaction(decimal amount, long createdBy)
    {
        var txn = new PFSettlementTxn(PfSettlementId, EmpSysId, amount, createdBy);
        _transactions.Add(txn);
    }

    public void Cancel()
    {
        if (PfSettlementStatus == TransactionStatus.Cancelled)
            throw new InvalidOperationException("Settlement is already cancelled.");
        PfSettlementStatus = TransactionStatus.Cancelled;
    }

    public void MarkCertified()
    {
        PfSettlementStatus = TransactionStatus.Posted;
    }
}
