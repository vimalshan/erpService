using MemberService.Domain.Common;
using MemberService.Domain.Enums;

namespace MemberService.Domain.Entities;

public class MemberNominee : BaseEntity
{
    public long MemberNo { get; private set; }
    public int SerialNo { get; private set; }
    public string FundType { get; private set; } = string.Empty;
    public string NomineeName { get; private set; } = string.Empty;
    public string RelationshipCode { get; private set; } = string.Empty;
    public long Percentage { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public string? AddressLine1 { get; private set; }
    public string? AddressLine2 { get; private set; }
    public string? AddressLine3 { get; private set; }
    public string? PhoneNo { get; private set; }
    public string? Email { get; private set; }
    public DateTime EffectiveDate { get; private set; }
    public DateTime? ClosureDate { get; private set; }
    public bool IsMinor { get; private set; }
    public string TrustCode { get; private set; } = string.Empty;
    public NomineeStatus Status { get; private set; } = NomineeStatus.Active;

    // Navigation
    public NomineeGuardian? Guardian { get; private set; }

    private MemberNominee() { }

    public static MemberNominee Create(long memberNo, int serialNo, string fundType, string nomineeName,
        string relationshipCode, long percentage, DateTime dob, bool isMinor, string trustCode,
        string? addressLine1 = null, string? phoneNo = null, string? email = null)
    {
        if (percentage < 0 || percentage > 100)
            throw new ArgumentException("Percentage must be between 0 and 100.", nameof(percentage));
        if (string.IsNullOrWhiteSpace(nomineeName))
            throw new ArgumentException("Nominee name is required.", nameof(nomineeName));

        return new MemberNominee
        {
            MemberNo = memberNo,
            SerialNo = serialNo,
            FundType = fundType,
            NomineeName = nomineeName,
            RelationshipCode = relationshipCode,
            Percentage = percentage,
            DateOfBirth = dob,
            IsMinor = isMinor,
            TrustCode = trustCode,
            AddressLine1 = addressLine1,
            PhoneNo = phoneNo,
            Email = email,
            EffectiveDate = DateTime.UtcNow,
            Status = NomineeStatus.Active
        };
    }

    public void Deactivate()
    {
        Status = NomineeStatus.Inactive;
        ClosureDate = DateTime.UtcNow;
    }
}
