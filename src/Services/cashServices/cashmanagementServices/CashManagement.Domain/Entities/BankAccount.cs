using CashManagement.Domain.Common;
using CashManagement.Domain.Events;
using CashManagement.Domain.ValueObjects;

namespace CashManagement.Domain.Entities;

public class BankAccount : AggregateRoot
{
    public string BankName { get; private set; } = default!;
    public string AccountNo { get; private set; } = default!;
    public string? Branch { get; private set; }
    public string? AccountType { get; private set; }
    public EntityStatus Status { get; private set; }
    public long CreatedBy { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public long? UpdatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }

    private readonly List<BankTransaction> _transactions = new();
    public IReadOnlyCollection<BankTransaction> Transactions => _transactions.AsReadOnly();

    private readonly List<ChequeRegister> _cheques = new();
    public IReadOnlyCollection<ChequeRegister> Cheques => _cheques.AsReadOnly();

    private BankAccount() { }

    public static BankAccount Create(long id, string bankName, string accountNo, string? branch,
        string? accountType, long createdBy)
    {
        var account = new BankAccount
        {
            Id = id,
            BankName = bankName,
            AccountNo = accountNo,
            Branch = branch,
            AccountType = accountType,
            Status = EntityStatus.Active,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };
        account.AddDomainEvent(new BankAccountCreatedEvent(account.Id, account.BankName, account.AccountNo));
        return account;
    }

    public void Deactivate(long updatedBy)
    {
        Status = EntityStatus.Inactive;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }

    public void Activate(long updatedBy)
    {
        Status = EntityStatus.Active;
        UpdatedBy = updatedBy;
        UpdatedOn = DateTime.UtcNow;
    }
}
