using TrainingDevelopment.Domain.Common;

namespace TrainingDevelopment.Domain.Entities;

/// <summary>
/// Maps to INSTITUTE_MASTER table.
/// </summary>
public class InstituteMaster : AuditableEntity
{
    public decimal InstituteCode { get; private set; }          // INSTITUTE_CODE
    public string? InstituteName { get; private set; }          // INSTITUTE_NAME
    public string? Address1 { get; private set; }               // INSTITUTE_ADD1
    public string? Address2 { get; private set; }               // INSTITUTE_ADD2
    public string? City { get; private set; }                   // INSTITUTE_CITY
    public string? State { get; private set; }                  // INSTITUTE_STATE
    public string? Pin { get; private set; }                    // INSTITUTE_PIN
    public string? Phone { get; private set; }                  // INSTITUTE_PHONE
    public string? Fax { get; private set; }                    // INSTITUTE_FAX
    public string? Email { get; private set; }                  // INSTITUTE_EMAIL
    public string? Url { get; private set; }                    // INSTITUTE_URL
    public string? InstituteType { get; private set; }          // INSTITUTE_TYPE
    public string CampusRecruit { get; private set; } = "N";  // INSTITUTE_CAMPUSRECRUIT
    public string? InstituteClass { get; private set; }         // INSTITUTE_CLASS

    private InstituteMaster() { }

    public static InstituteMaster Create(
        decimal code,
        string? name,
        string? address1,
        string? address2,
        string? city,
        string? state,
        string? pin,
        string? phone,
        string? fax,
        string? email,
        string? url,
        string? instituteType,
        string campusRecruit,
        string? instituteClass,
        decimal? modifiedBy)
    {
        return new InstituteMaster
        {
            InstituteCode = code,
            InstituteName = name,
            Address1 = address1,
            Address2 = address2,
            City = city,
            State = state,
            Pin = pin,
            Phone = phone,
            Fax = fax,
            Email = email,
            Url = url,
            InstituteType = instituteType,
            CampusRecruit = campusRecruit,
            InstituteClass = instituteClass,
            LastModifiedBy = modifiedBy,
            LastModifiedOn = DateTime.UtcNow
        };
    }

    public void Update(string? name, string? address1, string? address2,
        string? city, string? state, string? pin, string? phone,
        string? fax, string? email, string? url, string? instituteType,
        string campusRecruit, string? instituteClass, decimal? modifiedBy)
    {
        InstituteName = name;
        Address1 = address1;
        Address2 = address2;
        City = city;
        State = state;
        Pin = pin;
        Phone = phone;
        Fax = fax;
        Email = email;
        Url = url;
        InstituteType = instituteType;
        CampusRecruit = campusRecruit;
        InstituteClass = instituteClass;
        LastModifiedBy = modifiedBy;
        LastModifiedOn = DateTime.UtcNow;
    }
}
