namespace LoanTransaction.Domain.ValueObjects;

/// <summary>RBM=Remaining Balance; EM1=Equal Monthly 1st; EMA=Equal Monthly Auto; FPI=Fixed Principal+Interest</summary>
public sealed class RecoveryMethod : IEquatable<RecoveryMethod>
{
    public const string RemainingBalance = "RBM";
    public const string EqualMonthly1st = "EM1";
    public const string EqualMonthlyAuto = "EMA";
    public const string FixedPrincipalInterest = "FPI";

    public string Value { get; }

    private RecoveryMethod(string value) => Value = value;

    public static RecoveryMethod FromValue(string value)
    {
        var valid = new[] { RemainingBalance, EqualMonthly1st, EqualMonthlyAuto, FixedPrincipalInterest };
        if (!valid.Contains(value))
            throw new ArgumentException($"Invalid recovery method: {value}", nameof(value));
        return new(value);
    }

    public override bool Equals(object? obj) => Equals(obj as RecoveryMethod);
    public bool Equals(RecoveryMethod? other) => other is not null && Value == other.Value;
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value;
}
