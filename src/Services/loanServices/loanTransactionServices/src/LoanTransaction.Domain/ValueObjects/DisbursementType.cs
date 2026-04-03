namespace LoanTransaction.Domain.ValueObjects;

/// <summary>NEW = New loan; ADJ = Adjust against old loan</summary>
public sealed class DisbursementType : IEquatable<DisbursementType>
{
    public const string New = "NEW";
    public const string Adjustment = "ADJ";

    public string Value { get; }

    private DisbursementType(string value) => Value = value;

    public static DisbursementType NewLoan() => new(New);
    public static DisbursementType Adjust() => new(Adjustment);
    public static DisbursementType FromValue(string value)
    {
        if (value != New && value != Adjustment)
            throw new ArgumentException($"Invalid disbursement type: {value}", nameof(value));
        return new(value);
    }

    public bool IsNew => Value == New;
    public bool IsAdjustment => Value == Adjustment;

    public override bool Equals(object? obj) => Equals(obj as DisbursementType);
    public bool Equals(DisbursementType? other) => other is not null && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}
