namespace LoanApplication.Domain.ValueObjects;

/// <summary>
/// Loan Source value object (DIR/SLF)
/// </summary>
public class LoanSource : IEquatable<LoanSource>
{
    public const string DirectorateSource = "DIR";  // Directorate Loan
    public const string SelfLoanSource = "SLF";     // Self Loan

    public string Value { get; private set; }

    private LoanSource(string source)
    {
        if (!IsValidSource(source))
            throw new ArgumentException($"Invalid loan source: {source}");

        Value = source;
    }

    public static LoanSource Directorate() => new(DirectorateSource);
    public static LoanSource SelfLoan() => new(SelfLoanSource);
    public static LoanSource FromValue(string value) => new(value);

    public bool IsDirectorate => Value == DirectorateSource;
    public bool IsSelfLoan => Value == SelfLoanSource;

    private static bool IsValidSource(string source) =>
        source is DirectorateSource or SelfLoanSource;

    public override bool Equals(object? obj) => Equals(obj as LoanSource);

    public bool Equals(LoanSource? other) =>
        other is not null && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;
}
