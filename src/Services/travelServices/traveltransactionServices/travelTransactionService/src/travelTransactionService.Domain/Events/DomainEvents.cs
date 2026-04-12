using travelTransactionService.Domain.Common;

namespace travelTransactionService.Domain.Events;

public sealed class VendorCreatedEvent : IDomainEvent
{
    public long VendorId { get; }
    public string VendorName { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public VendorCreatedEvent(long vendorId, string vendorName)
    {
        VendorId = vendorId;
        VendorName = vendorName;
    }
}

public sealed class TaxMasterCreatedEvent : IDomainEvent
{
    public long VendorId { get; }
    public string TaxType { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public TaxMasterCreatedEvent(long vendorId, string taxType)
    {
        VendorId = vendorId;
        TaxType = taxType;
    }
}

public sealed class JaiInterfaceLineCreatedEvent : IDomainEvent
{
    public string TransactionNum { get; }
    public decimal TransactionLineNum { get; }
    public DateTime OccurredOn { get; } = DateTime.UtcNow;

    public JaiInterfaceLineCreatedEvent(string transactionNum, decimal transactionLineNum)
    {
        TransactionNum = transactionNum;
        TransactionLineNum = transactionLineNum;
    }
}
