using GSTComplianceService.Domain.Common;

namespace GSTComplianceService.Domain.Entities;

public class GstStateRegDetail : BaseEntity
{
    public long GstTinId { get; private set; }
    public long GstId { get; private set; }
    public string? GstState { get; private set; }
    public string? GstAddress { get; private set; }
    public string? GstVendCity { get; private set; }
    public string? GstVendCityName { get; private set; }
    public string? GstVendPincode { get; private set; }
    public string? GstTinNo { get; private set; }
    public string? GstExcNo { get; private set; }
    public string? GstSerNo { get; private set; }
    public string? GstGstinNo { get; private set; }
    public string? GstArnNo { get; private set; }
    public string? GstArnCopy { get; private set; }
    public string? GstArnTempFile { get; private set; }
    public string? GstContactPerson { get; private set; }
    public string? GstEmailId { get; private set; }
    public string? GstMobileNo { get; private set; }
    public string? GstRemarks { get; private set; }

    public GstMain? GstMain { get; private set; }

    private GstStateRegDetail() { }

    public static GstStateRegDetail Create(long gstId, string state, string? address,
        string? gstinNo, string? tinNo) =>
        new()
        {
            GstId = gstId,
            GstState = state,
            GstAddress = address,
            GstGstinNo = gstinNo,
            GstTinNo = tinNo
        };

    public void SetContactInfo(string? contactPerson, string? email, string? mobile)
    {
        GstContactPerson = contactPerson;
        GstEmailId = email;
        GstMobileNo = mobile;
    }
}
