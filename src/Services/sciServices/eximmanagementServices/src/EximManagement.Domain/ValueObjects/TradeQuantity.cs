using EximManagement.Domain.Common;

namespace EximManagement.Domain.ValueObjects;

/// <summary>Value object representing a trade quantity with unit.</summary>
public class TradeQuantity : ValueObject
{
    public decimal Amount { get; }
    public string Unit { get; }

    private TradeQuantity(decimal amount, string unit)
    {
        Amount = amount;
        Unit = unit.ToUpperInvariant();
    }

    public static TradeQuantity Create(decimal amount, string unit)
    {
        if (amount < 0) throw new ArgumentException("Quantity cannot be negative.", nameof(amount));
        if (string.IsNullOrWhiteSpace(unit)) throw new ArgumentException("Unit is required.", nameof(unit));
        return new TradeQuantity(amount, unit);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Unit;
    }

    public override string ToString() => $"{Amount} {Unit}";
}
