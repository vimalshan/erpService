using EmployeeService.Domain.Common;
using EmployeeService.Domain.Events;
using EmployeeService.Domain.ValueObjects;

namespace EmployeeService.Domain.Entities;

public sealed class Employee : AggregateRoot
{
    public int EmployeeId { get; private set; }
    public int? UserId { get; private set; }
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public EmployeeCode EmployeeCode { get; private set; } = null!;
    public DateTime HireDate { get; private set; }
    public string? JobTitle { get; private set; }
    public string? Department { get; private set; }
    public int? WarehouseId { get; private set; }
    public PhoneNumber? Phone { get; private set; }
    public Email? Email { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedDate { get; private set; }
    public DateTime ModifiedDate { get; private set; }

    private Employee() { } // EF Core constructor

    public static Employee Create(
        string firstName,
        string lastName,
        string employeeCode,
        DateTime hireDate,
        string? jobTitle = null,
        string? department = null,
        int? userId = null,
        int? warehouseId = null,
        string? phone = null,
        string? email = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required.", nameof(firstName));
        if (firstName.Length > 50)
            throw new ArgumentException("First name cannot exceed 50 characters.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name is required.", nameof(lastName));
        if (lastName.Length > 50)
            throw new ArgumentException("Last name cannot exceed 50 characters.", nameof(lastName));

        var employee = new Employee
        {
            FirstName = firstName,
            LastName = lastName,
            EmployeeCode = ValueObjects.EmployeeCode.Create(employeeCode),
            HireDate = hireDate,
            JobTitle = jobTitle,
            Department = department,
            UserId = userId,
            WarehouseId = warehouseId,
            Phone = PhoneNumber.Create(phone),
            Email = ValueObjects.Email.Create(email),
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        employee.AddDomainEvent(new EmployeeCreatedEvent(
            employee.EmployeeId,
            employee.EmployeeCode.Value,
            $"{employee.FirstName} {employee.LastName}"));

        return employee;
    }

    public void Update(
        string firstName,
        string lastName,
        DateTime hireDate,
        string? jobTitle,
        string? department,
        int? userId,
        int? warehouseId,
        string? phone,
        string? email)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required.", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name is required.", nameof(lastName));

        FirstName = firstName;
        LastName = lastName;
        HireDate = hireDate;
        JobTitle = jobTitle;
        Department = department;
        UserId = userId;
        WarehouseId = warehouseId;
        Phone = PhoneNumber.Create(phone);
        Email = ValueObjects.Email.Create(email);
        ModifiedDate = DateTime.UtcNow;

        AddDomainEvent(new EmployeeUpdatedEvent(EmployeeId, EmployeeCode.Value));
    }

    public void Deactivate()
    {
        IsActive = false;
        ModifiedDate = DateTime.UtcNow;
        AddDomainEvent(new EmployeeDeactivatedEvent(EmployeeId, EmployeeCode.Value));
    }

    public void Activate()
    {
        IsActive = true;
        ModifiedDate = DateTime.UtcNow;
    }
}
