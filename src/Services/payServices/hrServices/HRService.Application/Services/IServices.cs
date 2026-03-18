using HRService.Domain.Entities;
using MediatR;

namespace HRService.Application.Services;

/// <summary>
/// Interface for domain event publisher
/// </summary>
public interface IDomainEventPublisher
{
    Task PublishAsync(Domain.Common.DomainEvent domainEvent, CancellationToken cancellationToken = default);
    Task PublishAsync(IEnumerable<Domain.Common.DomainEvent> domainEvents, CancellationToken cancellationToken = default);
}

/// <summary>
/// Interface for employee service
/// </summary>
public interface IEmployeeService
{
    Task<Guid> CreateEmployeeAsync(DTOs.CreateEmployeeDto dto, CancellationToken cancellationToken = default);
    Task<DTOs.EmployeeDto> GetEmployeeByIdAsync(Guid employeeId, CancellationToken cancellationToken = default);
    Task<List<DTOs.EmployeeDto>> GetAllEmployeesAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
    Task<bool> TerminateEmployeeAsync(Guid employeeId, DateTime terminationDate, string reason, CancellationToken cancellationToken = default);
    Task<bool> UpdateEmployeePositionAsync(Guid employeeId, Guid positionId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Interface for leave service
/// </summary>
public interface ILeaveService
{
    Task<Guid> RequestLeaveAsync(DTOs.RequestLeaveDto dto, CancellationToken cancellationToken = default);
    Task<DTOs.LeaveDto> GetLeaveByIdAsync(Guid leaveId, CancellationToken cancellationToken = default);
    Task<List<DTOs.LeaveDto>> GetEmployeeLeavesAsync(Guid employeeId, CancellationToken cancellationToken = default);
    Task<bool> ApproveLeaveAsync(Guid leaveId, Guid approvedBy, CancellationToken cancellationToken = default);
    Task<bool> RejectLeaveAsync(Guid leaveId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Interface for attendance service
/// </summary>
public interface IAttendanceService
{
    Task<Guid> MarkAttendanceAsync(DTOs.MarkAttendanceDto dto, CancellationToken cancellationToken = default);
    Task<DTOs.AttendanceDto> GetAttendanceByIdAsync(Guid attendanceId, CancellationToken cancellationToken = default);
    Task<List<DTOs.AttendanceDto>> GetEmployeeAttendanceAsync(Guid employeeId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}

/// <summary>
/// Interface for salary service
/// </summary>
public interface ISalaryService
{
    Task<Guid> CreateSalaryAsync(DTOs.UpdateSalaryDto dto, CancellationToken cancellationToken = default);
    Task<DTOs.SalaryDto> GetEmployeeSalaryAsync(Guid employeeId, CancellationToken cancellationToken = default);
    Task<bool> UpdateSalaryAsync(Guid employeeId, Guid salaryId, decimal newSalary, CancellationToken cancellationToken = default);
}
