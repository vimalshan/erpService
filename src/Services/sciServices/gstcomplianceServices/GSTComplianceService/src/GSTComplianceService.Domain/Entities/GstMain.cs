using GSTComplianceService.Domain.Common;
using GSTComplianceService.Domain.Enums;

namespace GSTComplianceService.Domain.Entities;

public class GstMain : BaseEntity
{
    public long GstId { get; private set; }
    public char? GstType { get; private set; }
    public string GstPanNo { get; private set; } = string.Empty;
    public string? GstEmailId { get; private set; }
    public long? GstMobileNo { get; private set; }
    public DateTime GstCreatedOn { get; private set; }
    public DateTime? GstModifiedOn { get; private set; }
    public long? GstVendorId { get; private set; }
    public char? GstVendorNameFlag { get; private set; }
    public string? GstVendorName { get; private set; }
    public int? GstVendConst { get; private set; }
    public char? GstVendAddFlag { get; private set; }
    public string? GstVendAddLine1 { get; private set; }
    public string? GstVendAddLine2 { get; private set; }
    public string? GstVendAddLine3 { get; private set; }
    public string? GstVendAddLine4 { get; private set; }
    public string? GstVendCity { get; private set; }
    public string? GstVendCityName { get; private set; }
    public string? GstVendState { get; private set; }
    public string? GstVendPincode { get; private set; }
    public int? GstRegistrationType { get; private set; }
    public string? GstContactName { get; private set; }
    public string? GstContactEmailId { get; private set; }
    public long? GstContactMobileNo { get; private set; }
    public string? GstRemarks { get; private set; }
    public char? GstStatus { get; private set; }
    public string GstDigitalFlag { get; private set; } = "N";
    public string? GstGstnCopy { get; private set; }
    public char? GstEnteredByFlag { get; private set; }
    public long? GstEnteredBy { get; private set; }
    public char? GstScreenType { get; private set; }

    // Navigation properties
    public ICollection<GstHsnDetail> HsnDetails { get; private set; } = new List<GstHsnDetail>();
    public ICollection<GstServiceDetail> ServiceDetails { get; private set; } = new List<GstServiceDetail>();
    public ICollection<GstStateRegDetail> StateRegDetails { get; private set; } = new List<GstStateRegDetail>();

    // EF Core constructor
    private GstMain() { }

    public static GstMain Create(string panNo, char? type, string? email, long? mobile, long registeredBy)
    {
        var entity = new GstMain
        {
            GstPanNo = panNo,
            GstType = type,
            GstEmailId = email,
            GstMobileNo = mobile,
            GstCreatedOn = DateTime.UtcNow,
            GstEnteredBy = registeredBy,
            GstStatus = 'P',
            GstDigitalFlag = "N"
        };
        entity.AddDomainEvent(new Events.GstRegisteredEvent(entity.GstId, panNo, DateTime.UtcNow));
        return entity;
    }

    public void UpdateVendorInfo(string? vendorName, string? addLine1, string? addLine2,
        string? city, string? state, string? pincode)
    {
        GstVendorName = vendorName;
        GstVendAddLine1 = addLine1;
        GstVendAddLine2 = addLine2;
        GstVendCity = city;
        GstVendState = state;
        GstVendPincode = pincode;
        GstModifiedOn = DateTime.UtcNow;
    }

    public void Activate()
    {
        var previous = GstStatus;
        GstStatus = 'A';
        GstModifiedOn = DateTime.UtcNow;
        AddDomainEvent(new Events.GstStatusChangedEvent(GstId, previous, GstStatus.Value, DateTime.UtcNow));
    }

    public void Deactivate()
    {
        var previous = GstStatus;
        GstStatus = 'I';
        GstModifiedOn = DateTime.UtcNow;
        AddDomainEvent(new Events.GstStatusChangedEvent(GstId, previous, GstStatus.Value, DateTime.UtcNow));
    }

    public void SetGstinCopy(string? gstinCopy)
    {
        GstGstnCopy = gstinCopy;
        GstModifiedOn = DateTime.UtcNow;
    }
}
