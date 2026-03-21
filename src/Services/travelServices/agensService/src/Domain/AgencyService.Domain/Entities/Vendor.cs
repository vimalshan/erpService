using AgencyService.Domain.Common;

namespace AgencyService.Domain.Entities;

public class Vendor : AggregateRoot
{
    public string Name { get; private set; }
    public string CategoryType { get; private set; } // V=Vendor, H=Hotel
    public string? Phone { get; private set; }
    public string? AddressLine1 { get; private set; }
    public string? AddressLine2 { get; private set; }
    public string? AddressLine3 { get; private set; }
    public string? AddressLine4 { get; private set; }
    public string? AddressLine5 { get; private set; }
    public long? CityCode { get; private set; }
    public string? PAN { get; private set; }
    public string? AccountNumber { get; private set; }
    public string? BankName { get; private set; }
    
    public Vendor(
        long vendorId,
        string name,
        string categoryType,
        string? phone = null,
        string? addressLine1 = null)
    {
        if (vendorId <= 0)
            throw new ArgumentException("Vendor ID must be greater than 0", nameof(vendorId));
            
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Vendor name cannot be empty", nameof(name));
            
        if (categoryType != "V" && categoryType != "H")
            throw new ArgumentException("Category type must be V (Vendor) or H (Hotel)", nameof(categoryType));
            
        Id = vendorId;
        Name = name;
        CategoryType = categoryType;
        Phone = phone;
        AddressLine1 = addressLine1;
        
        AddDomainEvent(new VendorCreatedEvent(vendorId, name, categoryType));
    }
    
    public void Update(string name, string? phone, long? cityCode)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Vendor name cannot be empty", nameof(name));
            
        Name = name;
        Phone = phone;
        CityCode = cityCode;
        
        AddDomainEvent(new VendorUpdatedEvent(Id, name));
    }
    
    private Vendor() { }
}

public class VendorCreatedEvent : DomainEvent
{
    public long VendorId { get; set; }
    public string VendorName { get; set; }
    public string CategoryType { get; set; }
    
    public VendorCreatedEvent(long vendorId, string vendorName, string categoryType)
    {
        VendorId = vendorId;
        VendorName = vendorName;
        CategoryType = categoryType;
    }
}

public class VendorUpdatedEvent : DomainEvent
{
    public long VendorId { get; set; }
    public string VendorName { get; set; }
    
    public VendorUpdatedEvent(long vendorId, string vendorName)
    {
        VendorId = vendorId;
        VendorName = vendorName;
    }
}
