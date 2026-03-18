namespace SwipeTransactionService.Domain.ValueObjects;

public sealed record CompanyCode
{
    public long Value { get; }

    public CompanyCode(long value)
    {
        if (value <= 0) throw new ArgumentException("Company code must be positive.", nameof(value));
        Value = value;
    }

    public static implicit operator long(CompanyCode code) => code.Value;
    public static implicit operator CompanyCode(long value) => new(value);
    public override string ToString() => Value.ToString();
}
