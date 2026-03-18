using BankService.Domain.Common;

namespace BankService.Domain.Entities;

public class BankAccount : AggregateRoot
{
    public long AccountId { get; private set; }
    public string AccountNumber { get; private set; } = null!;
    public string AccountTitle { get; private set; } = null!;
    public string BankCode { get; private set; } = null!;
    public string TrustCode { get; private set; } = null!;
    public string AccountType { get; private set; } = null!;
    public decimal AccountBalance { get; private set; }
    public string AccountStatus { get; private set; } = "A";
    public DateTime OpeningDate { get; private set; }
    public DateTime? ClosingDate { get; private set; }

    // Navigation
    public ICollection<ChequeRegister> ChequeRegisters { get; private set; } = [];

    private BankAccount() { }

    public static BankAccount Create(string accountNumber, string accountTitle,
        string bankCode, string trustCode, string accountType, DateTime openingDate)
    {
        var account = new BankAccount
        {
            AccountNumber = accountNumber,
            AccountTitle = accountTitle,
            BankCode = bankCode,
            TrustCode = trustCode,
            AccountType = accountType,
            AccountBalance = 0,
            AccountStatus = "A",
            OpeningDate = openingDate
        };

        account.AddDomainEvent(new Events.BankAccountCreatedEvent(accountNumber, accountTitle));
        return account;
    }

    public void UpdateBalance(decimal newBalance)
    {
        AccountBalance = newBalance;
    }

    public void Close(DateTime closingDate)
    {
        ClosingDate = closingDate;
        AccountStatus = "C";
    }

    public void Activate()
    {
        ClosingDate = null;
        AccountStatus = "A";
    }
}
