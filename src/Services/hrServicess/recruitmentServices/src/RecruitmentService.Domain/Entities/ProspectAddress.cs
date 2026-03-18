namespace RecruitmentService.Domain.Entities;

public class ProspectAddress
{
    public decimal EmpSysId { get; private set; }
    public string AddressFlag { get; private set; } = default!; // C=Current, P=Permanent
    public string? Address1 { get; private set; }
    public string? Address2 { get; private set; }
    public string? Address3 { get; private set; }
    public string? Address4 { get; private set; }
    public decimal? City { get; private set; }
    public decimal? PinCode { get; private set; }
    public decimal? UpdatedBy { get; private set; }
    public DateTime? UpdatedOn { get; private set; }
    public string? MobileNo { get; private set; }
    public string? LandlineNo { get; private set; }

    private ProspectAddress() { }

    public static ProspectAddress Create(
        decimal empSysId, string flag, string? addr1, string? addr2,
        string? addr3, string? addr4, decimal? city, decimal? pinCode,
        string? mobile, string? landline, decimal? updatedBy) =>
        new()
        {
            EmpSysId = empSysId,
            AddressFlag = flag,
            Address1 = addr1,
            Address2 = addr2,
            Address3 = addr3,
            Address4 = addr4,
            City = city,
            PinCode = pinCode,
            MobileNo = mobile,
            LandlineNo = landline,
            UpdatedBy = updatedBy,
            UpdatedOn = DateTime.UtcNow
        };
}
