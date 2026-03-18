using InsuranceManagement.Domain.Common;

namespace InsuranceManagement.Domain.ValueObjects;

public class EnrollmentStatus : ValueObject
{
    public const string Active = "A";
    public const string Inactive = "I";
    public const string Suspended = "S";
    public const string Terminated = "T";

    public string Value { get; }

    private EnrollmentStatus(string value)
    {
        Value = value;
    }

    public static EnrollmentStatus Active_Status => new(Active);
    public static EnrollmentStatus Inactive_Status => new(Inactive);
    public static EnrollmentStatus Suspended_Status => new(Suspended);
    public static EnrollmentStatus Terminated_Status => new(Terminated);

    public static EnrollmentStatus Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Status cannot be empty", nameof(value));

        return new EnrollmentStatus(value.ToUpper());
    }

    public bool IsActive => Value == Active;
    public bool IsInactive => Value == Inactive;
    public bool IsSuspended => Value == Suspended;
    public bool IsTerminated => Value == Terminated;

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
