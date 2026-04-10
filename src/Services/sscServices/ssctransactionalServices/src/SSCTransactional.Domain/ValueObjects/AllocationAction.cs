using SSCTransactional.Domain.Common;

namespace SSCTransactional.Domain.ValueObjects;

/// <summary>
/// Allocation action type: M=Processing(Maker), C=Validation(Checker), P=Payments
/// </summary>
public sealed class AllocationAction : ValueObject
{
    public static readonly AllocationAction Processing = new("M");
    public static readonly AllocationAction Validation = new("C");
    public static readonly AllocationAction Payments = new("P");

    public string Value { get; }

    private AllocationAction(string value) => Value = value;

    public static AllocationAction From(string value)
    {
        return value.ToUpperInvariant() switch
        {
            "M" => Processing,
            "C" => Validation,
            "P" => Payments,
            _ => throw new ArgumentException($"Invalid allocation action: {value}. Must be M, C, or P.")
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
