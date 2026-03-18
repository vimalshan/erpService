namespace HRService.Domain.Entities;

public class Department : Common.AggregateRoot
{
    public string DepartmentCode { get; private set; } = null!;
    public string DepartmentName { get; private set; } = null!;
    public string? Description { get; private set; }
    public Guid? ManagerId { get; private set; }

    private Department() { }

    public static Department Create(string departmentCode, string departmentName, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(departmentCode))
            throw new ArgumentException("Department code cannot be empty", nameof(departmentCode));

        if (string.IsNullOrWhiteSpace(departmentName))
            throw new ArgumentException("Department name cannot be empty", nameof(departmentName));

        return new Department
        {
            Id = Guid.NewGuid(),
            DepartmentCode = departmentCode,
            DepartmentName = departmentName,
            Description = description,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };
    }

    public void UpdateManager(Guid managerId)
    {
        ManagerId = managerId;
        ModifiedDate = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        ModifiedDate = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        ModifiedDate = DateTime.UtcNow;
    }
}
