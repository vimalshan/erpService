using MemberService.Domain.Common;
using MemberService.Domain.Enums;

namespace MemberService.Domain.Entities;

public class NomineeGuardian : BaseEntity
{
    public string TrustCode { get; private set; } = string.Empty;
    public long NomineeMemberNo { get; private set; }
    public long NomineeSerialNo { get; private set; }
    public string GuardianName { get; private set; } = string.Empty;
    public string? AddressLine1 { get; private set; }
    public string? AddressLine2 { get; private set; }
    public string? AddressLine3 { get; private set; }
    public string? AddressLine4 { get; private set; }
    public string? PhoneNo { get; private set; }
    public string? Email { get; private set; }
    public DateTime? EffectiveDate { get; private set; }
    public DateTime? ClosureDate { get; private set; }
    public string GuardianRelationship { get; private set; } = string.Empty;
    public char Status { get; private set; } = 'A';

    private NomineeGuardian() { }

    public static NomineeGuardian Create(string trustCode, long nomineeMemberNo, long nomineeSerialNo,
        string guardianName, string relationship, string? addressLine1 = null,
        string? phoneNo = null, string? email = null)
    {
        if (string.IsNullOrWhiteSpace(guardianName))
            throw new ArgumentException("Guardian name is required.", nameof(guardianName));

        return new NomineeGuardian
        {
            TrustCode = trustCode,
            NomineeMemberNo = nomineeMemberNo,
            NomineeSerialNo = nomineeSerialNo,
            GuardianName = guardianName,
            GuardianRelationship = relationship,
            AddressLine1 = addressLine1,
            PhoneNo = phoneNo,
            Email = email,
            EffectiveDate = DateTime.UtcNow,
            Status = 'A'
        };
    }
}
