namespace HRService.Domain.Entities;

public enum EmploymentStatus
{
    Active,
    OnLeave,
    Suspended,
    Terminated
}

public enum EmploymentType
{
    Permanent,
    Contract,
    Probation
}

public class Employee : Common.AggregateRoot
{
    public string EmployeeCode { get; private set; } = null!;
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public string? MiddleName { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public string? Gender { get; private set; }
    public ValueObjects.Email Email { get; private set; } = null!;
    public ValueObjects.PhoneNumber? PhoneNumber { get; private set; }
    public string? SSN { get; private set; }
    public Guid DepartmentId { get; private set; }
    public Guid PositionId { get; private set; }
    public Guid? ManagerId { get; private set; }
    public Guid SiteId { get; private set; }
    public DateTime JoinDate { get; private set; }
    public DateTime? TerminationDate { get; private set; }
    public EmploymentStatus Status { get; private set; } = EmploymentStatus.Active;
    public EmploymentType EmploymentType { get; private set; }
    public Guid? ReportingManagerId { get; private set; }

    private Employee() { }

    public static Employee Create(
        string employeeCode,
        string firstName,
        string lastName,
        DateTime dateOfBirth,
        string email,
        Guid departmentId,
        Guid positionId,
        Guid siteId,
        DateTime joinDate,
        EmploymentType employmentType,
        string? middleName = null,
        string? gender = null,
        string? phoneNumber = null,
        string? ssn = null,
        Guid? managerId = null)
    {
        if (string.IsNullOrWhiteSpace(employeeCode))
            throw new ArgumentException("Employee code cannot be empty", nameof(employeeCode));

        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty", nameof(lastName));

        if (DateTime.Today.AddYears(-18) < dateOfBirth)
            throw new ArgumentException("Employee must be at least 18 years old", nameof(dateOfBirth));

        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            EmployeeCode = employeeCode,
            FirstName = firstName,
            LastName = lastName,
            MiddleName = middleName,
            DateOfBirth = dateOfBirth,
            Gender = gender,
            Email = ValueObjects.Email.Create(email),
            PhoneNumber = !string.IsNullOrWhiteSpace(phoneNumber) ? ValueObjects.PhoneNumber.Create(phoneNumber) : null,
            SSN = ssn,
            DepartmentId = departmentId,
            PositionId = positionId,
            ManagerId = managerId,
            SiteId = siteId,
            JoinDate = joinDate,
            EmploymentType = employmentType,
            Status = EmploymentStatus.Active,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        var @event = new Events.EmployeeCreatedEvent
        {
            EmployeeId = employee.Id,
            EmployeeCode = employeeCode,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            JoinDate = joinDate
        };

        employee.AddDomainEvent(@event);

        return employee;
    }

    public void Terminate(DateTime terminationDate, string reason)
    {
        if (terminationDate < JoinDate)
            throw new InvalidOperationException("Termination date cannot be before join date");

        TerminationDate = terminationDate;
        Status = EmploymentStatus.Terminated;
        IsActive = false;
        ModifiedDate = DateTime.UtcNow;

        var @event = new Events.EmployeeTerminatedEvent
        {
            EmployeeId = Id,
            TerminationDate = terminationDate,
            Reason = reason
        };

        AddDomainEvent(@event);
    }

    public void Suspend()
    {
        Status = EmploymentStatus.Suspended;
        ModifiedDate = DateTime.UtcNow;
    }

    public void Resume()
    {
        if (TerminationDate.HasValue)
            throw new InvalidOperationException("Cannot resume a terminated employee");

        Status = EmploymentStatus.Active;
        ModifiedDate = DateTime.UtcNow;
    }

    public void UpdatePosition(Guid positionId)
    {
        PositionId = positionId;
        ModifiedDate = DateTime.UtcNow;
    }

    public void UpdateDepartment(Guid departmentId)
    {
        DepartmentId = departmentId;
        ModifiedDate = DateTime.UtcNow;
    }

    public string GetFullName() => $"{FirstName} {LastName}";
}
