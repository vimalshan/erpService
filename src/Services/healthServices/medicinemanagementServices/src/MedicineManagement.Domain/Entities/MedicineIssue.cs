using MedicineManagement.Domain.Common;

namespace MedicineManagement.Domain.Entities;

public class MedicineIssue : AuditableEntity, IAggregateRoot
{
    public string? CompanyCode { get; private set; }
    public string? TransactionNumber { get; private set; }
    public string? TransactionDate { get; private set; }
    public long? IssuedQuantity { get; private set; }
    public string? VisitNumber { get; private set; }
    public string? MedicineCode { get; private set; }

    private MedicineIssue() { }

    public static MedicineIssue Create(
        string companyCode, string transactionNumber, string medicineCode,
        long issuedQuantity, string visitNumber, string entryUser, string entryUserPin)
    {
        var entity = new MedicineIssue
        {
            CompanyCode = companyCode,
            TransactionNumber = transactionNumber,
            TransactionDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
            MedicineCode = medicineCode,
            IssuedQuantity = issuedQuantity,
            VisitNumber = visitNumber,
            EntryUser = entryUser,
            EntryDate = DateTime.UtcNow
        };
        entity.AddDomainEvent(new Events.MedicineIssuedEvent(entity));
        return entity;
    }
}
