using InsuranceManagement.Domain.Common;

namespace InsuranceManagement.Domain.ValueObjects;

public class ClaimType : ValueObject
{
    public const string InPatient = "IN_PATIENT";
    public const string OutPatient = "OUT_PATIENT";
    public const string Dental = "DENTAL";
    public const string Optical = "OPTICAL";

    public string Value { get; }

    private ClaimType(string value)
    {
        Value = value;
    }

    public static ClaimType InPatient_Claim => new(InPatient);
    public static ClaimType OutPatient_Claim => new(OutPatient);
    public static ClaimType Dental_Claim => new(Dental);
    public static ClaimType Optical_Claim => new(Optical);

    public static ClaimType Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Claim type cannot be empty", nameof(value));

        return new ClaimType(value.ToUpper());
    }

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
