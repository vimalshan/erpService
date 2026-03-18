namespace PayrollServices.Domain.ValueObjects;

/// <summary>
/// Value object representing salary components
/// </summary>
public class SalaryComponents : IEquatable<SalaryComponents>
{
    public decimal BasicPay { get; init; }
    public decimal Allowances { get; init; }
    public decimal Deductions { get; init; }

    public decimal GrossSalary => BasicPay + Allowances;
    public decimal NetSalary => GrossSalary - Deductions;

    public SalaryComponents(decimal basicPay, decimal allowances, decimal deductions)
    {
        if (basicPay < 0)
            throw new ArgumentException("Basic pay cannot be negative.", nameof(basicPay));
        if (allowances < 0)
            throw new ArgumentException("Allowances cannot be negative.", nameof(allowances));
        if (deductions < 0)
            throw new ArgumentException("Deductions cannot be negative.", nameof(deductions));

        BasicPay = basicPay;
        Allowances = allowances;
        Deductions = deductions;
    }

    public override bool Equals(object? obj) => Equals(obj as SalaryComponents);

    public bool Equals(SalaryComponents? other)
    {
        return other is not null &&
               BasicPay == other.BasicPay &&
               Allowances == other.Allowances &&
               Deductions == other.Deductions;
    }

    public override int GetHashCode() => HashCode.Combine(BasicPay, Allowances, Deductions);

    public override string ToString() => $"Basic: {BasicPay:C}, Allowances: {Allowances:C}, Deductions: {Deductions:C}, Net: {NetSalary:C}";
}
