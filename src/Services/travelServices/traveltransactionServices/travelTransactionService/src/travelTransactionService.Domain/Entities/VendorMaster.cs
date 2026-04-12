using travelTransactionService.Domain.Common;

namespace travelTransactionService.Domain.Entities;

public class VendorMaster : AggregateRoot
{
    public long VendorId { get; private set; }
    public string Name { get; private set; } = null!;
    public string? AddressLine1 { get; private set; }
    public string? AddressLine2 { get; private set; }
    public string? AddressLine3 { get; private set; }
    public string? AddressLine4 { get; private set; }
    public string? AddressLine5 { get; private set; }
    public long? CityCode { get; private set; }
    public string? ItPanNumber { get; private set; }
    public string? PhoneNumber { get; private set; }
    public string? AccountNumber { get; private set; }
    public string? BankName { get; private set; }
    public string CategoryType { get; private set; } = null!;

    private VendorMaster() { }

    public static VendorMaster Create(
        long vendorId,
        string name,
        string categoryType,
        string? addressLine1 = null,
        string? phone = null,
        string? panNumber = null)
    {
        var vendor = new VendorMaster
        {
            VendorId = vendorId,
            Name = name,
            CategoryType = categoryType,
            AddressLine1 = addressLine1,
            PhoneNumber = phone,
            ItPanNumber = panNumber
        };

        vendor.AddDomainEvent(new Events.VendorCreatedEvent(vendorId, name));
        return vendor;
    }

    public void Update(string name, string? address1, string? phone, string? pan, string? bankName, string? accountNo)
    {
        Name = name;
        AddressLine1 = address1;
        PhoneNumber = phone;
        ItPanNumber = pan;
        BankName = bankName;
        AccountNumber = accountNo;
    }
}
