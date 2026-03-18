using MediatR;

namespace HRService.Application.Commands;

/// <summary>
/// Command to create a new employee
/// </summary>
public class CreateEmployeeCommand : IRequest<Guid>
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

public class TerminateEmployeeCommand : IRequest<bool>
{
    public Guid EmployeeId { get; set; }
    public DateTime TerminationDate { get; set; }
    public string Reason { get; set; }
}

public class UpdateEmployeePositionCommand : IRequest<bool>
{
    public Guid EmployeeId { get; set; }
    public Guid PositionId { get; set; }
}

public class SuspendEmployeeCommand : IRequest<bool>
{
    public Guid EmployeeId { get; set; }
}

public class ResumeEmployeeCommand : IRequest<bool>
{
    public Guid EmployeeId { get; set; }
}
