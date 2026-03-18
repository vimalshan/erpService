using CashManagement.Domain.Common;
using CashManagement.Domain.Events;
using CashManagement.Domain.Exceptions;
using CashManagement.Domain.ValueObjects;

namespace CashManagement.Domain.Entities;

public class CashTransaction : BaseEntity
{
    public long CashTxnId { get; private set; }
    public long CashUnitId { get; private set; }
    public CashTransactionType TxnType { get; private set; }
    public decimal Amount { get; private set; }
    public string? Source { get; private set; }
    public long? PayeeId { get; private set; }
    public string? RefNo { get; private set; }
    public DateTime TxnDate { get; private set; }
    public string? Remarks { get; private set; }
    public TransactionStatus Status { get; private set; }
    public long? AuthorizedBy { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }

    private CashTransaction() { }

    public static CashTransaction CreateReceipt(long cashUnitId, decimal amount,
        string? source, string? refNo, string? remarks, long createdBy, long? authorizedBy = null)
    {
        if (amount <= 0) throw new DomainException("Receipt amount must be greater than zero.");
        var txn = new CashTransaction
        {
            CashUnitId = cashUnitId,
            TxnType = CashTransactionType.Receipt,
            Amount = amount,
            Source = source,
            RefNo = refNo,
            TxnDate = DateTime.UtcNow,
            Remarks = remarks,
            Status = TransactionStatus.Posted,
            AuthorizedBy = authorizedBy,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };
        txn.AddDomainEvent(new CashReceiptRecordedEvent(txn.CashUnitId, txn.Amount, txn.RefNo));
        return txn;
    }

    public static CashTransaction CreateDisbursement(long cashUnitId, decimal amount,
        string? source, long? payeeId, string? refNo, string? remarks, long createdBy, long? authorizedBy = null)
    {
        if (amount <= 0) throw new DomainException("Disbursement amount must be greater than zero.");
        var txn = new CashTransaction
        {
            CashUnitId = cashUnitId,
            TxnType = CashTransactionType.Disbursement,
            Amount = amount,
            Source = source,
            PayeeId = payeeId,
            RefNo = refNo,
            TxnDate = DateTime.UtcNow,
            Remarks = remarks,
            Status = TransactionStatus.Posted,
            AuthorizedBy = authorizedBy,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };
        txn.AddDomainEvent(new CashDisbursementRecordedEvent(txn.CashUnitId, txn.Amount, txn.RefNo));
        return txn;
    }
}
