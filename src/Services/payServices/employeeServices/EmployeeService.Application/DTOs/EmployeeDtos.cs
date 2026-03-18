using System;

namespace EmployeeService.Application.DTOs;

/// <summary>
/// DTO for Employee read operations
/// </summary>
public class EmployeeDto
{
    public long Id { get; set; }
    public long EmployeeSystemId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? MiddleName { get; set; }
    public string Email { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string? CostCenterId { get; set; }
    
    public decimal GrossCTC { get; set; }
    public decimal BasicSalary { get; set; }
    public DateTime? CTCEffectiveDate { get; set; }
    
    public string EmploymentStatus { get; set; } = string.Empty;
    public DateTime JoiningDate { get; set; }
    public DateTime? TerminationDate { get; set; }
    
    public DateTime LastCTCModificationDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// DTO for creating an employee
/// </summary>
public class CreateEmployeeDto
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
/// DTO for updating employee information
/// </summary>
public class UpdateEmployeeDto
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
/// DTO for salary increment request
/// </summary>
public class SalaryIncrementRequestDto
{
    public long EmployeeSystemId { get; set; }
    public decimal IncrementPercentage { get; set; }
    public DateTime EffectiveDate { get; set; }
}

/// <summary>
/// DTO for salary increment log
/// </summary>
public class SalaryIncrementLogDto
{
    public long Id { get; set; }
    public long EmployeeSystemId { get; set; }
    public decimal OldCTC { get; set; }
    public decimal NewCTC { get; set; }
    public decimal IncrementPercentage { get; set; }
    public DateTime EffectiveDate { get; set; }
    public long ApprovedBy { get; set; }
    public DateTime ApprovedOn { get; set; }
    public string? ApprovalComments { get; set; }
    public string Status { get; set; } = string.Empty;
}
