namespace TaxService.Domain.ValueObjects;

/// <summary>
/// Value object representing a tax rate with bracket information
/// </summary>
public sealed record TaxRate(decimal Rate, decimal FromAmount, decimal ToAmount) 
    : IEquatable<TaxRate>
{
    public bool IsApplicable(decimal income) 
        => income >= FromAmount && income <= ToAmount;

    public decimal CalculateTax(decimal income)
    {
        if (!IsApplicable(income))
            return 0m;
        
        var taxableAmount = income - FromAmount;
        return taxableAmount * (Rate / 100);
    }

    public bool Equals(TaxRate? other)
        => other is not null && 
           Rate == other.Rate && 
           FromAmount == other.FromAmount && 
           ToAmount == other.ToAmount;

    public override int GetHashCode() 
        => HashCode.Combine(Rate, FromAmount, ToAmount);
}
