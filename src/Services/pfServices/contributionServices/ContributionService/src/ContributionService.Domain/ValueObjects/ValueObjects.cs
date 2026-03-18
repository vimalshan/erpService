namespace ContributionService.Domain.ValueObjects;

public record Money(decimal Amount)
{
    public static Money Zero => new Money(0m);

    public static Money Add(Money a, Money b) => new Money(a.Amount + b.Amount);
    public static Money Subtract(Money a, Money b) => new Money(a.Amount - b.Amount);
}

public record DateRange(DateTime Start, DateTime End)
{
    public bool Contains(DateTime date) => date >= Start && date <= End;
    public int TotalDays => (End - Start).Days;
}

public record ContributionRate(decimal EmployeeRate, decimal EmployerRate)
{
    public static ContributionRate Default => new(12.0m, 12.0m);

    public decimal CalculateEmployeeContribution(decimal basicSalary)
        => Math.Round(basicSalary * (EmployeeRate / 100), 0);

    public decimal CalculateEmployerContribution(decimal basicSalary)
        => Math.Round(basicSalary * (EmployerRate / 100), 0);

    public decimal CalculateTotalContribution(decimal basicSalary)
        => CalculateEmployeeContribution(basicSalary) + CalculateEmployerContribution(basicSalary);
}

public record BatchStatus
{
    public string Code { get; init; } = null!;
    public string Description { get; init; } = null!;

    public static BatchStatus Pending => new() { Code = "P", Description = "Pending" };
    public static BatchStatus Posted => new() { Code = "PO", Description = "Posted" };
    public static BatchStatus Approved => new() { Code = "A", Description = "Approved" };
    public static BatchStatus Rejected => new() { Code = "R", Description = "Rejected" };
}
