using ConfigService.Domain.Common;

namespace ConfigService.Domain.Entities;

public class TravelContact : AggregateRoot<string>
{
    public string? ContactType { get; private set; }
    public string? AdminId { get; private set; }
    public string? AdminName { get; private set; }
    public string? EmployeeSysId { get; private set; }
    public string? PhoneNos { get; private set; }
    public string? EmailId { get; private set; }
    public string? LastModifiedBy { get; private set; }
    public DateTime? LastModifiedOn { get; private set; }

    private TravelContact() { }

    public static TravelContact Create(string id, string? contactType, string? adminId,
        string? adminName, string? empSysId, string? phoneNos, string? emailId, string? modifiedBy)
    {
        return new TravelContact
        {
            Id = id, ContactType = contactType, AdminId = adminId, AdminName = adminName,
            EmployeeSysId = empSysId, PhoneNos = phoneNos, EmailId = emailId,
            LastModifiedBy = modifiedBy, LastModifiedOn = DateTime.UtcNow
        };
    }

    public void Update(string? contactType, string? adminName, string? phoneNos, string? emailId, string? modifiedBy)
    {
        ContactType = contactType;
        AdminName = adminName;
        PhoneNos = phoneNos;
        EmailId = emailId;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
