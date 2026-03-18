namespace StipendService.Domain.ValueObjects;

public sealed class StipendAmount
{
    public decimal Value { get; }

    private StipendAmount(decimal value) => Value = value;

    public static StipendAmount Create(decimal value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Stipend amount cannot be negative.");
        return new StipendAmount(value);
    }

    public static implicit operator decimal(StipendAmount amount) => amount.Value;

    public override string ToString() => Value.ToString("F2");

    public override bool Equals(object? obj) =>
        obj is StipendAmount other && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();
}
