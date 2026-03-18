using MemberService.Domain.Common;
using MemberService.Domain.Enums;

namespace MemberService.Domain.Entities;

public class MemberContact : BaseEntity
{
    public long ContactId { get; private set; }
    public long MemberNo { get; private set; }
    public ContactType ContactType { get; private set; }
    public string AddressLine1 { get; private set; } = string.Empty;
    public string? AddressLine2 { get; private set; }
    public string? AddressLine3 { get; private set; }
    public string City { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public string PinCode { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;
    public string? PhoneNo { get; private set; }
    public string? Email { get; private set; }
    public DateTime EffectiveDate { get; private set; }
    public DateTime? ClosureDate { get; private set; }

    private MemberContact() { }

    public static MemberContact Create(long memberNo, ContactType contactType, string addressLine1,
        string city, string state, string pinCode, string country,
        string? addressLine2 = null, string? addressLine3 = null,
        string? phoneNo = null, string? email = null) =>
        new()
        {
            MemberNo = memberNo,
            ContactType = contactType,
            AddressLine1 = addressLine1,
            AddressLine2 = addressLine2,
            AddressLine3 = addressLine3,
            City = city,
            State = state,
            PinCode = pinCode,
            Country = country,
            PhoneNo = phoneNo,
            Email = email,
            EffectiveDate = DateTime.UtcNow
        };

    public void Close() => ClosureDate = DateTime.UtcNow;
}
