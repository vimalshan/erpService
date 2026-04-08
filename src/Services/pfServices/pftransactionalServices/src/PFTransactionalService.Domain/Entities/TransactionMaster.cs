using PFTransactionalService.Domain.Common;

namespace PFTransactionalService.Domain.Entities;

public class TransactionMaster : BaseEntity
{
    public string TransactionTrustCode { get; private set; } = string.Empty;
    public string TransactionCode { get; private set; } = string.Empty;
    public string TransactionName { get; private set; } = string.Empty;
    public string TransactionType { get; private set; } = string.Empty;
    public string TransactionValue { get; private set; } = string.Empty;

    private TransactionMaster() { }

    public TransactionMaster(string trustCode, string code, string name, string type, string value)
    {
        TransactionTrustCode = trustCode;
        TransactionCode = code;
        TransactionName = name;
        TransactionType = type;
        TransactionValue = value;
    }
}
