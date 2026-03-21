using ShipmentService.Domain.Common;

namespace ShipmentService.Domain.ValueObjects;

public sealed class ShippingCost : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }

    private ShippingCost(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static ShippingCost Create(decimal amount, string currency = "USD")
    {
        if (amount < 0)
            throw new ArgumentException("Shipping cost cannot be negative.", nameof(amount));
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be empty.", nameof(currency));
        return new ShippingCost(Math.Round(amount, 2), currency.ToUpper());
    }

    /// <summary>Calculates shipping cost based on weight and service type (mirrors fn_CalculateShippingCost).</summary>
    public static ShippingCost Calculate(decimal weight, string serviceType)
    {
        var amount = serviceType switch
        {
            "Express" => 10.00m + (weight * 2.50m),
            "Standard" => 5.00m + (weight * 1.50m),
            _ => 7.00m + (weight * 2.00m)
        };
        return Create(amount);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString() => $"{Currency} {Amount:F2}";
}
