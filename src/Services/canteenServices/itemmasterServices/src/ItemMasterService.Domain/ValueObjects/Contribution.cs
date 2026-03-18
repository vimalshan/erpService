namespace ItemMasterService.Domain.ValueObjects;

public sealed class Contribution : IEquatable<Contribution>
{
    public decimal EmployeeAmount { get; }
    public decimal EmployerAmount { get; }

    private Contribution(decimal employeeAmount, decimal employerAmount)
    {
        EmployeeAmount = employeeAmount;
        EmployerAmount = employerAmount;
    }

    public static Contribution Create(decimal employeeAmount, decimal employerAmount)
    {
        if (employeeAmount < 0) throw new ArgumentException("Employee contribution cannot be negative.", nameof(employeeAmount));
        if (employerAmount < 0) throw new ArgumentException("Employer contribution cannot be negative.", nameof(employerAmount));
        return new Contribution(employeeAmount, employerAmount);
    }

    public bool Equals(Contribution? other) =>
        other is not null && EmployeeAmount == other.EmployeeAmount && EmployerAmount == other.EmployerAmount;

    public override bool Equals(object? obj) => obj is Contribution c && Equals(c);
    public override int GetHashCode() => HashCode.Combine(EmployeeAmount, EmployerAmount);
}
