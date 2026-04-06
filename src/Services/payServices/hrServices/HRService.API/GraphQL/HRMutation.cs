using HRService.Application.Commands;
using MediatR;

namespace HRService.API.GraphQL;

public class HRMutation
{
    // ─── Employee ────────────────────────────────────────────────────────────

    [GraphQLName("createEmployee")]
    public async Task<Guid> CreateEmployee(
        string employeeCode,
        string firstName,
        string lastName,
        string? middleName,
        DateTime dateOfBirth,
        string? gender,
        string email,
        string? phoneNumber,
        Guid departmentId,
        Guid positionId,
        Guid siteId,
        DateTime joinDate,
        string employmentType,
        Guid? managerId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        var command = new CreateEmployeeCommand
        {
            EmployeeCode = employeeCode,
            FirstName = firstName,
            LastName = lastName,
            MiddleName = middleName,
            DateOfBirth = dateOfBirth,
            Gender = gender,
            Email = email,
            PhoneNumber = phoneNumber,
            DepartmentId = departmentId,
            PositionId = positionId,
            SiteId = siteId,
            JoinDate = joinDate,
            EmploymentType = employmentType,
            ManagerId = managerId
        };
        return await mediator.Send(command, cancellationToken);
    }

    [GraphQLName("terminateEmployee")]
    public async Task<bool> TerminateEmployee(
        Guid employeeId,
        DateTime terminationDate,
        string reason,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new TerminateEmployeeCommand
        {
            EmployeeId = employeeId,
            TerminationDate = terminationDate,
            Reason = reason
        }, cancellationToken);
    }

    [GraphQLName("updateEmployeePosition")]
    public async Task<bool> UpdateEmployeePosition(
        Guid employeeId,
        Guid positionId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new UpdateEmployeePositionCommand
        {
            EmployeeId = employeeId,
            PositionId = positionId
        }, cancellationToken);
    }

    [GraphQLName("suspendEmployee")]
    public async Task<bool> SuspendEmployee(
        Guid employeeId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new SuspendEmployeeCommand { EmployeeId = employeeId }, cancellationToken);
    }

    [GraphQLName("resumeEmployee")]
    public async Task<bool> ResumeEmployee(
        Guid employeeId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new ResumeEmployeeCommand { EmployeeId = employeeId }, cancellationToken);
    }

    // ─── Department ──────────────────────────────────────────────────────────

    [GraphQLName("createDepartment")]
    public async Task<Guid> CreateDepartment(
        string departmentCode,
        string departmentName,
        string? description,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new CreateDepartmentCommand
        {
            DepartmentCode = departmentCode,
            DepartmentName = departmentName,
            Description = description
        }, cancellationToken);
    }

    [GraphQLName("updateDepartmentManager")]
    public async Task<bool> UpdateDepartmentManager(
        Guid departmentId,
        Guid managerId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new UpdateDepartmentManagerCommand
        {
            DepartmentId = departmentId,
            ManagerId = managerId
        }, cancellationToken);
    }

    [GraphQLName("deactivateDepartment")]
    public async Task<bool> DeactivateDepartment(
        Guid departmentId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new DeactivateDepartmentCommand { DepartmentId = departmentId }, cancellationToken);
    }

    // ─── Leave ───────────────────────────────────────────────────────────────

    [GraphQLName("requestLeave")]
    public async Task<Guid> RequestLeave(
        Guid employeeId,
        Guid leaveTypeId,
        DateTime startDate,
        DateTime endDate,
        string? reason,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new RequestLeaveCommand
        {
            EmployeeId = employeeId,
            LeaveTypeId = leaveTypeId,
            StartDate = startDate,
            EndDate = endDate,
            Reason = reason
        }, cancellationToken);
    }

    [GraphQLName("approveLeave")]
    public async Task<bool> ApproveLeave(
        Guid leaveId,
        Guid approvedBy,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new ApproveLeaveCommand
        {
            LeaveId = leaveId,
            ApprovedBy = approvedBy
        }, cancellationToken);
    }

    [GraphQLName("rejectLeave")]
    public async Task<bool> RejectLeave(
        Guid leaveId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new RejectLeaveCommand { LeaveId = leaveId }, cancellationToken);
    }

    [GraphQLName("cancelLeave")]
    public async Task<bool> CancelLeave(
        Guid leaveId,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new CancelLeaveCommand { LeaveId = leaveId }, cancellationToken);
    }

    // ─── Attendance ──────────────────────────────────────────────────────────

    [GraphQLName("markAttendance")]
    public async Task<Guid> MarkAttendance(
        Guid employeeId,
        DateTime attendanceDate,
        Guid shiftId,
        TimeSpan? checkInTime,
        TimeSpan? checkOutTime,
        string status,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new MarkAttendanceCommand
        {
            EmployeeId = employeeId,
            AttendanceDate = attendanceDate,
            ShiftId = shiftId,
            CheckInTime = checkInTime,
            CheckOutTime = checkOutTime,
            Status = status
        }, cancellationToken);
    }

    [GraphQLName("updateAttendance")]
    public async Task<bool> UpdateAttendance(
        Guid attendanceId,
        TimeSpan? checkInTime,
        TimeSpan? checkOutTime,
        string status,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new UpdateAttendanceCommand
        {
            AttendanceId = attendanceId,
            CheckInTime = checkInTime,
            CheckOutTime = checkOutTime,
            Status = status
        }, cancellationToken);
    }

    // ─── Salary ──────────────────────────────────────────────────────────────

    [GraphQLName("createSalary")]
    public async Task<Guid> CreateSalary(
        Guid employeeId,
        DateTime effectiveDate,
        decimal totalBaseSalary,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new CreateSalaryCommand
        {
            EmployeeId = employeeId,
            EffectiveDate = effectiveDate,
            TotalBaseSalary = totalBaseSalary
        }, cancellationToken);
    }

    [GraphQLName("updateEmployeeSalary")]
    public async Task<Guid> UpdateEmployeeSalary(
        Guid employeeId,
        DateTime effectiveDate,
        decimal newSalary,
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
    {
        return await mediator.Send(new UpdateEmployeeSalaryCommand
        {
            EmployeeId = employeeId,
            EffectiveDate = effectiveDate,
            NewSalary = newSalary
        }, cancellationToken);
    }
}
