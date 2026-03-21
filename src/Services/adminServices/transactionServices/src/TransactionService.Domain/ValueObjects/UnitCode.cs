namespace TransactionService.Domain.ValueObjects;

using TransactionService.Domain.Common;

public sealed class UnitCode : ValueObject
{
    public string Value { get; }

    public UnitCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Unit code cannot be empty.");
        if (value.Trim().Length > 3)
            throw new ArgumentException("Unit code cannot exceed 3 characters.");
        Value = value.Trim().ToUpperInvariant();
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public static implicit operator string(UnitCode code) => code.Value;
    public static implicit operator UnitCode(string value) => new(value);
}
