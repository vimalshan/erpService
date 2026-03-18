using System;

namespace EmployeeService.Domain.ValueObjects;

/// <summary>
/// Value object for percentage values
/// </summary>
public class Percentage : IEquatable<Percentage>
{
    public decimal Value { get; private set; }

    private Percentage() { }

    public Percentage(decimal value)
    {
        if (value < 0 || value > 100)
            throw new ArgumentException("Percentage must be between 0 and 100", nameof(value));

        Value = decimal.Round(value, 2);
    }

    public decimal CalculatePercentageOf(decimal amount)
    {
        return decimal.Round(amount * (Value / 100), 2);
    }

    public decimal ApplyPercentage(decimal amount)
    {
        return decimal.Round(amount * (1 + (Value / 100)), 2);
    }

    public override bool Equals(object? obj)
    {
        return obj is Percentage percentage && Equals(percentage);
    }

    public bool Equals(Percentage? other)
    {
        return other != null && Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return $"{Value:N2}%";
    }
}
