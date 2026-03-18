namespace HRService.Application.DTOs;

public class EmployeeDto
{
    public Guid EmployeeId { get; set; }
    public string EmployeeCode { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public Guid DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public Guid PositionId { get; set; }
    public string? PositionTitle { get; set; }
    public DateTime JoinDate { get; set; }
    public DateTime? TerminationDate { get; set; }
    public string Status { get; set; }
    public string EmploymentType { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}

public class CreateEmployeeDto
{
    public string EmployeeCode { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? MiddleName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public Guid DepartmentId { get; set; }
    public Guid PositionId { get; set; }
    public Guid SiteId { get; set; }
    public DateTime JoinDate { get; set; }
    public string EmploymentType { get; set; }
    public Guid? ManagerId { get; set; }
}

public class UpdateEmployeeDto
{
    public Guid EmployeeId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public Guid? PositionId { get; set; }
    public Guid? DepartmentId { get; set; }
    public string? PhoneNumber { get; set; }
}

public class TerminateEmployeeDto
{
    public Guid EmployeeId { get; set; }
    public DateTime TerminationDate { get; set; }
    public string Reason { get; set; }
}
