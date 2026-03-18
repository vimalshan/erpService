using AccountingService.Domain.Common;

namespace AccountingService.Domain.Entities;

/// <summary>Maps to TRANSACTION_MASTER table – transaction type definitions.</summary>
public class TransactionMaster : BaseEntity
{
    public string TransactionTrustCode { get; private set; } = default!;
    public string TransactionCode { get; private set; } = default!;
    public string TransactionName { get; private set; } = default!;
    public string TransactionType { get; private set; } = default!;
    public string TransactionValue { get; private set; } = default!;

    private TransactionMaster() { }

    public static TransactionMaster Create(
        string trustCode, string code, string name,
        string type, string value)
    {
        return new TransactionMaster
        {
            TransactionTrustCode = trustCode,
            TransactionCode = code,
            TransactionName = name,
            TransactionType = type,
            TransactionValue = value
        };
    }
}
