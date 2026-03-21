using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class Vendor : AggregateRoot<string>
{
    public string VendorName { get; private set; } = string.Empty;
    public string ActiveStatus { get; private set; } = string.Empty;
    public string VendorCode { get; private set; } = string.Empty;
    public string ContactPerson { get; private set; } = string.Empty;
    public string Address1 { get; private set; } = string.Empty;
    public string Address2 { get; private set; } = string.Empty;
    public string Address3 { get; private set; } = string.Empty;
    public string Address4 { get; private set; } = string.Empty;
    public string PinCode { get; private set; } = string.Empty;
    public string EmailId { get; private set; } = string.Empty;
    public string CcEmailId { get; private set; } = string.Empty;
    public string SrfTriggerId { get; private set; } = string.Empty;
    public string MobileNo { get; private set; } = string.Empty;
    public string PhoneNos { get; private set; } = string.Empty;
    public string VendorType { get; private set; } = string.Empty;
    public string SubType { get; private set; } = string.Empty;
    public string? DirectMail { get; private set; }
    public string? UserId { get; private set; }
    public string? GstNo { get; private set; }

    private readonly List<VendorTaxRate> _taxRates = [];
    public IReadOnlyCollection<VendorTaxRate> TaxRates => _taxRates.AsReadOnly();

    private readonly List<VendorUnitMap> _unitMaps = [];
    public IReadOnlyCollection<VendorUnitMap> UnitMaps => _unitMaps.AsReadOnly();

    private readonly List<VendorCharges> _charges = [];
    public IReadOnlyCollection<VendorCharges> Charges => _charges.AsReadOnly();

    private Vendor() { }

    public static Vendor Create(string id, string name, string active, string code,
        string contactPerson, string address1, string address2, string address3, string address4,
        string pinCode, string emailId, string ccEmailId, string srfTriggerId,
        string mobileNo, string phoneNos, string vendorType, string subType)
    {
        var entity = new Vendor
        {
            Id = id, VendorName = name, ActiveStatus = active, VendorCode = code,
            ContactPerson = contactPerson, Address1 = address1, Address2 = address2,
            Address3 = address3, Address4 = address4, PinCode = pinCode,
            EmailId = emailId, CcEmailId = ccEmailId, SrfTriggerId = srfTriggerId,
            MobileNo = mobileNo, PhoneNos = phoneNos, VendorType = vendorType, SubType = subType
        };
        entity.AddDomainEvent(new Events.VendorCreatedEvent(id, name));
        return entity;
    }

    public void Update(string name, string active, string contactPerson, string emailId, string mobileNo)
    {
        VendorName = name;
        ActiveStatus = active;
        ContactPerson = contactPerson;
        EmailId = emailId;
        MobileNo = mobileNo;
        AddDomainEvent(new Events.VendorUpdatedEvent(Id, name));
    }

    public void AddTaxRate(VendorTaxRate rate) => _taxRates.Add(rate);
    public void AddUnitMap(VendorUnitMap map) => _unitMaps.Add(map);
    public void AddCharge(VendorCharges charge) => _charges.Add(charge);
}
