namespace CanteenTransactionService.Domain.ValueObjects;

public sealed class CompanyCode : IEquatable<CompanyCode>
{
    public long Value { get; }

    private CompanyCode(long value) => Value = value;

    public static CompanyCode Create(long value)
    {
        if (value <= 0) throw new ArgumentException("Company code must be positive.", nameof(value));
        return new CompanyCode(value);
    }

    public bool Equals(CompanyCode? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is CompanyCode vo && Equals(vo);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();
    public static implicit operator long(CompanyCode code) => code.Value;
}
