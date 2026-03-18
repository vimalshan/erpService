namespace CanteenUnit.Domain.ValueObjects;

public sealed class CompanyCode : IEquatable<CompanyCode>
{
    public decimal Value { get; }

    private CompanyCode(decimal value) => Value = value;

    public static CompanyCode Create(decimal value)
    {
        if (value <= 0)
            throw new ArgumentException("Company code must be a positive number.", nameof(value));
        return new CompanyCode(value);
    }

    public bool Equals(CompanyCode? other) => other is not null && Value == other.Value;
    public override bool Equals(object? obj) => obj is CompanyCode cc && Equals(cc);
    public override int GetHashCode() => Value.GetHashCode();
    public override string ToString() => Value.ToString();

    public static implicit operator decimal(CompanyCode cc) => cc.Value;
}
