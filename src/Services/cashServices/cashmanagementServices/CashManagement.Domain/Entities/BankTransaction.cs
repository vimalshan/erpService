using CashManagement.Domain.Common;
using CashManagement.Domain.Events;
using CashManagement.Domain.Exceptions;
using CashManagement.Domain.ValueObjects;

namespace CashManagement.Domain.Entities;

public class BankTransaction : BaseEntity
{
    public long BankTxnId { get; private set; }
    public long BankAccountId { get; private set; }
    public BankTransactionType TxnType { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime TxnDate { get; private set; }
    public string? Reference { get; private set; }
    public string? Remarks { get; private set; }
    public TransactionStatus Status { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }

    private BankTransaction() { }

    public static BankTransaction Create(long bankAccountId, BankTransactionType type,
        decimal amount, string? reference, string? remarks, long createdBy)
    {
        if (amount <= 0) throw new DomainException("Transaction amount must be greater than zero.");
        var txn = new BankTransaction
        {
            BankAccountId = bankAccountId,
            TxnType = type,
            Amount = amount,
            TxnDate = DateTime.UtcNow,
            Reference = reference,
            Remarks = remarks,
            Status = TransactionStatus.Posted,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };
        txn.AddDomainEvent(new BankTransactionRecordedEvent(txn.BankAccountId, txn.TxnType.ToString(), txn.Amount));
        return txn;
    }
}
