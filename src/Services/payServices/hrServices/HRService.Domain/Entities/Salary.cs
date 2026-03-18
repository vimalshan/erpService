namespace HRService.Domain.Entities;

public class SalaryComponent : Common.AggregateRoot
{
    public string ComponentName { get; private set; } = null!;
    public string ComponentType { get; private set; } = null!; // Basic, HRA, DA, Allowance, Deduction

    private SalaryComponent() { }

    public static SalaryComponent Create(string componentName, string componentType)
    {
        if (string.IsNullOrWhiteSpace(componentName))
            throw new ArgumentException("Component name cannot be empty", nameof(componentName));

        if (string.IsNullOrWhiteSpace(componentType))
            throw new ArgumentException("Component type cannot be empty", nameof(componentType));

        return new SalaryComponent
        {
            Id = Guid.NewGuid(),
            ComponentName = componentName,
            ComponentType = componentType,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
    }

    public void Deactivate()
    {
        IsActive = false;
        ModifiedDate = DateTime.UtcNow;
    }
}

public enum SalaryStatus
{
    Active,
    Inactive,
    Revised
}

public class EmployeeSalary : Common.AggregateRoot
{
    public Guid EmployeeId { get; private set; }
    public DateTime EffectiveDate { get; private set; }
    public decimal TotalBaseSalary { get; private set; }
    public SalaryStatus Status { get; private set; } = SalaryStatus.Active;
    public List<SalaryComponent> Components { get; private set; } = new();

    private EmployeeSalary() { }

    public static EmployeeSalary Create(
        Guid employeeId,
        DateTime effectiveDate,
        decimal totalBaseSalary)
    {
        if (employeeId == Guid.Empty)
            throw new ArgumentException("Employee id cannot be empty", nameof(employeeId));

        if (totalBaseSalary <= 0)
            throw new ArgumentException("Salary must be greater than zero", nameof(totalBaseSalary));

        if (effectiveDate > DateTime.Today)
            throw new ArgumentException("Effective date cannot be in the future");

        return new EmployeeSalary
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId,
            EffectiveDate = effectiveDate,
            TotalBaseSalary = totalBaseSalary,
            Status = SalaryStatus.Active,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
    }

    public void UpdateSalary(decimal newSalary)
    {
        if (newSalary <= 0)
            throw new ArgumentException("Salary must be greater than zero");

        TotalBaseSalary = newSalary;
        ModifiedDate = DateTime.UtcNow;

        var @event = new Events.SalaryUpdatedEvent
        {
            EmployeeId = EmployeeId,
            SalaryId = Id,
            NewSalary = newSalary,
            EffectiveDate = DateTime.UtcNow
        };

        AddDomainEvent(@event);
    }

    public void Deactivate()
    {
        Status = SalaryStatus.Inactive;
        IsActive = false;
        ModifiedDate = DateTime.UtcNow;
    }
}
