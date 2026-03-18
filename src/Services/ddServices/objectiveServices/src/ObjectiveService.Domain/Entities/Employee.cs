namespace ObjectiveService.Domain.Entities;

/// <summary>
/// Employee entity - represents an employee in the system
/// </summary>
public class Employee : BaseEntity
{
    public string UserId { get; set; }
    public decimal PinNumber { get; set; }
    public decimal EmployeeSysId { get; set; }
    public string Department { get; set; }
    public string Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }

    private Employee() { }

    public Employee(string userId, decimal pinNumber, decimal employeeSysId, string department)
    {
        UserId = userId;
        PinNumber = pinNumber;
        EmployeeSysId = employeeSysId;
        Department = department;
        Status = "A"; // Active
        CreatedDate = DateTime.UtcNow;
    }

    public void UpdateDepartment(string department)
    {
        Department = department;
        ModifiedDate = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        Status = "I"; // Inactive
        ModifiedDate = DateTime.UtcNow;
    }
}
