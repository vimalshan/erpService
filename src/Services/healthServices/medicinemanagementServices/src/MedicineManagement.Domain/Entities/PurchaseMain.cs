using MedicineManagement.Domain.Common;

namespace MedicineManagement.Domain.Entities;

public class PurchaseMain : AuditableEntity, IAggregateRoot
{
    public string CompanyCode { get; private set; } = null!;
    public long TransactionNumber { get; private set; }
    public string VendorName { get; private set; } = null!;
    public string InvoiceNumber { get; private set; } = null!;
    public DateTime InvoiceDate { get; private set; }
    public decimal InvoiceAmount { get; private set; }
    public char CancelFlag { get; private set; }

    // Navigation
    private readonly List<PurchaseSub> _lineItems = [];
    public IReadOnlyCollection<PurchaseSub> LineItems => _lineItems.AsReadOnly();

    private PurchaseMain() { }

    public static PurchaseMain Create(
        string companyCode, long transactionNumber, string vendorName,
        string invoiceNumber, DateTime invoiceDate, decimal invoiceAmount,
        string entryUser, decimal entryUserPin)
    {
        var entity = new PurchaseMain
        {
            CompanyCode = companyCode,
            TransactionNumber = transactionNumber,
            VendorName = vendorName,
            InvoiceNumber = invoiceNumber,
            InvoiceDate = invoiceDate,
            InvoiceAmount = invoiceAmount,
            CancelFlag = 'N',
            EntryUser = entryUser,
            EntryUserPin = entryUserPin,
            EntryDate = DateTime.UtcNow
        };
        entity.AddDomainEvent(new Events.PurchaseCreatedEvent(entity));
        return entity;
    }

    public void AddLineItem(PurchaseSub lineItem)
    {
        _lineItems.Add(lineItem);
    }

    public void Cancel(string modifiedUser, decimal modifiedUserPin)
    {
        CancelFlag = 'Y';
        ModifiedUser = modifiedUser;
        ModifiedUserPin = modifiedUserPin;
        ModifiedDate = DateTime.UtcNow;
    }
}
