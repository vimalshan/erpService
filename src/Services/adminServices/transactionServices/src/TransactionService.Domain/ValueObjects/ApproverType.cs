namespace TransactionService.Domain.ValueObjects;

using TransactionService.Domain.Common;

public sealed class ApproverType : ValueObject
{
    public static readonly ApproverType Approver = new("A");
    public static readonly ApproverType Indentor = new("I");

    public string Value { get; }

    public ApproverType(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Trim().Length > 1)
            throw new ArgumentException("Approver type must be a single character (A or I).");
        Value = value.Trim().ToUpperInvariant();
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(ApproverType type) => type.Value;
    public static implicit operator ApproverType(string value) => new(value);
}
