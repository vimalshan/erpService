using BankService.Domain.Common;

namespace BankService.Domain.Entities;

public class ChequeRegister : BaseEntity
{
    public long RegisterId { get; private set; }
    public decimal ChequeNoFrom { get; private set; }
    public decimal ChequeNoTo { get; private set; }
    public string ChequeBookId { get; private set; } = null!;
    public long AccountId { get; private set; }
    public DateTime IssuedDate { get; private set; }
    public string RegisterStatus { get; private set; } = "A";

    // Navigation
    public BankAccount Account { get; private set; } = null!;

    private ChequeRegister() { }

    public static ChequeRegister Create(decimal chequeNoFrom, decimal chequeNoTo,
        string chequeBookId, long accountId, DateTime issuedDate)
    {
        if (chequeNoTo < chequeNoFrom)
            throw new InvalidOperationException("ChequeNoTo must be >= ChequeNoFrom.");

        return new ChequeRegister
        {
            ChequeNoFrom = chequeNoFrom,
            ChequeNoTo = chequeNoTo,
            ChequeBookId = chequeBookId,
            AccountId = accountId,
            IssuedDate = issuedDate,
            RegisterStatus = "A"
        };
    }

    public void Deactivate() => RegisterStatus = "I";
    public void Activate() => RegisterStatus = "A";
}
