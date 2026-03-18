using System;
using System.Threading;
using System.Threading.Tasks;
using EmployeeService.Application.DTOs;
using MediatR;

namespace EmployeeService.Application.Commands;

/// <summary>
/// Command to create a new employee
/// </summary>
public class CreateEmployeeCommand : IRequest<EmployeeDto>
{
    public long EmployeeSystemId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string? CostCenterId { get; set; }
    public DateTime JoiningDate { get; set; }
    public decimal GrossCTC { get; set; }
    public decimal BasicSalary { get; set; }
    public DateTime CTCEffectiveDate { get; set; }
}

/// <summary>
/// Command to update employee information
/// </summary>
public class UpdateEmployeeCommand : IRequest<EmployeeDto>
{
    public long EmployeeSystemId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? CostCenterId { get; set; }
}

/// <summary>
/// Command to process salary increment
/// </summary>
public class ProcessSalaryIncrementCommand : IRequest<SalaryIncrementLogDto>
{
    public long EmployeeSystemId { get; set; }
    public decimal IncrementPercentage { get; set; }
    public DateTime EffectiveDate { get; set; }
    public long ApprovedBy { get; set; }
}

/// <summary>
/// Command to modify employee CTC directly
/// </summary>
public class ModifyEmployeeCTCCommand : IRequest<EmployeeDto>
{
    public long EmployeeSystemId { get; set; }
    public decimal NewGrossCTC { get; set; }
    public decimal NewBasicSalary { get; set; }
    public DateTime EffectiveDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public long ModifiedBy { get; set; }
}

/// <summary>
/// Command to terminate employee
/// </summary>
public class TerminateEmployeeCommand : IRequest<EmployeeDto>
{
    public long EmployeeSystemId { get; set; }
    public DateTime TerminationDate { get; set; }
}

/// <summary>
/// Command to delete employee
/// </summary>
public class DeleteEmployeeCommand : IRequest<bool>
{
    public long EmployeeSystemId { get; set; }
}
