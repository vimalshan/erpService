using CompensationService.Domain.Common;

namespace CompensationService.Domain.ValueObjects;

/// <summary>
/// Value object for Salary and Benefits
/// </summary>
public sealed class SalaryStructure : ValueObject
{
    public decimal BaseSalary { get; }
    public decimal HraPercentage { get; }
    public decimal DaPercentage { get; }

    private SalaryStructure(decimal baseSalary, decimal hraPercentage, decimal daPercentage)
    {
        if (baseSalary <= 0)
            throw new ArgumentException("Base salary must be greater than zero", nameof(baseSalary));

        if (hraPercentage < 0 || hraPercentage > 100)
            throw new ArgumentException("HRA percentage must be between 0 and 100", nameof(hraPercentage));

        if (daPercentage < 0 || daPercentage > 100)
            throw new ArgumentException("DA percentage must be between 0 and 100", nameof(daPercentage));

        BaseSalary = baseSalary;
        HraPercentage = hraPercentage;
        DaPercentage = daPercentage;
    }

    public static SalaryStructure Create(decimal baseSalary, decimal hraPercentage, decimal daPercentage)
        => new(baseSalary, hraPercentage, daPercentage);

    public decimal CalculateHRA() => BaseSalary * (HraPercentage / 100);
    public decimal CalculateDA() => BaseSalary * (DaPercentage / 100);
    public decimal CalculateTotalSalary() => BaseSalary + CalculateHRA() + CalculateDA();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return BaseSalary;
        yield return HraPercentage;
        yield return DaPercentage;
    }
}
