using MedicineManagement.Domain.Common;

namespace MedicineManagement.Domain.Entities;

public class MedicineCredit : AuditableEntity, IAggregateRoot
{
    public string CompanyCode { get; private set; } = null!;
    public long TransactionCode { get; private set; }
    public string MedicineCode { get; private set; } = null!;
    public char RecordType { get; private set; } // O=Opening, P=Purchase, I=Issue, E=Expire
    public long Quantity { get; private set; }
    public DateTime TransactionDate { get; private set; }
    public string? LotNumber { get; private set; }
    public char? CancelFlag { get; private set; }
    public long? TransactionNumber { get; private set; }

    // Navigation
    public Medicine? Medicine { get; private set; }

    private MedicineCredit() { }

    public static MedicineCredit Create(
        string companyCode, long transactionCode, string medicineCode,
        char recordType, long quantity, DateTime transactionDate,
        string entryUser, decimal entryUserPin, string? lotNumber = null)
    {
        var entity = new MedicineCredit
        {
            CompanyCode = companyCode,
            TransactionCode = transactionCode,
            MedicineCode = medicineCode,
            RecordType = recordType,
            Quantity = quantity,
            TransactionDate = transactionDate,
            EntryUser = entryUser,
            EntryUserPin = entryUserPin,
            EntryDate = DateTime.UtcNow,
            LotNumber = lotNumber
        };
        entity.AddDomainEvent(new Events.StockTransactionCreatedEvent(entity));
        return entity;
    }

    public void Cancel(string modifiedUser, decimal modifiedUserPin)
    {
        CancelFlag = 'Y';
        ModifiedUser = modifiedUser;
        ModifiedUserPin = modifiedUserPin;
        ModifiedDate = DateTime.UtcNow;
    }
}
