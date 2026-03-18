namespace DeductionService.Domain.ValueObjects;

/// <summary>
/// Value object representing a payroll deduction amount with employee and employer shares.
/// </summary>
public sealed record DeductionAmount
{
    public decimal EmployeeShare { get; }
    public decimal EmployerShare { get; }
    public decimal Total => EmployeeShare + EmployerShare;

    private DeductionAmount(decimal employeeShare, decimal employerShare)
    {
        EmployeeShare = employeeShare;
        EmployerShare = employerShare;
    }

    public static DeductionAmount Create(decimal employeeShare, decimal employerShare)
    {
        if (employeeShare < 0)
            throw new ArgumentException("Employee share cannot be negative.", nameof(employeeShare));
        if (employerShare < 0)
            throw new ArgumentException("Employer share cannot be negative.", nameof(employerShare));

        return new DeductionAmount(employeeShare, employerShare);
    }

    public static DeductionAmount Zero => new(0, 0);
}
