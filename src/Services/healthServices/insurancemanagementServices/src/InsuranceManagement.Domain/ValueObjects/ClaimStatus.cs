using InsuranceManagement.Domain.Common;

namespace InsuranceManagement.Domain.ValueObjects;

public class ClaimStatus : ValueObject
{
    public const string Submitted = "S";
    public const string Approved = "A";
    public const string Rejected = "R";
    public const string Paid = "P";
    public const string Pending = "PND";

    public string Value { get; }

    private ClaimStatus(string value)
    {
        Value = value;
    }

    public static ClaimStatus Submitted_Status => new(Submitted);
    public static ClaimStatus Approved_Status => new(Approved);
    public static ClaimStatus Rejected_Status => new(Rejected);
    public static ClaimStatus Paid_Status => new(Paid);
    public static ClaimStatus Pending_Status => new(Pending);

    public static ClaimStatus Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Status cannot be empty", nameof(value));

        return new ClaimStatus(value.ToUpper());
    }

    public bool IsSubmitted => Value == Submitted;
    public bool IsApproved => Value == Approved;
    public bool IsRejected => Value == Rejected;
    public bool IsPaid => Value == Paid;
    public bool IsPending => Value == Pending;

    public override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
