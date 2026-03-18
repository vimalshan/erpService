using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EmployeeService.Domain.Entities;

namespace EmployeeService.Domain.Repositories;

/// <summary>
/// Repository interface for Employee aggregate
/// </summary>
public interface IEmployeeRepository
{
    /// <summary>
    /// Get employee by System ID
    /// </summary>
    Task<Employee?> GetBySystemIdAsync(long employeeSystemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get employee by ID
    /// </summary>
    Task<Employee?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all employees (active)
    /// </summary>
    Task<IReadOnlyList<Employee>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get employees by cost center
    /// </summary>
    Task<IReadOnlyList<Employee>> GetByCostCenterAsync(string costCenterId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add new employee
    /// </summary>
    Task<Employee> AddAsync(Employee employee, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update existing employee
    /// </summary>
    Task<Employee> UpdateAsync(Employee employee, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete employee
    /// </summary>
    Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check if employee exists
    /// </summary>
    Task<bool> ExistsAsync(long employeeSystemId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Repository interface for Salary Increment Log
/// </summary>
public interface ISalaryIncrementLogRepository
{
    /// <summary>
    /// Get increment log by ID
    /// </summary>
    Task<SalaryIncrementLog?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all increments for an employee
    /// </summary>
    Task<IReadOnlyList<SalaryIncrementLog>> GetByEmployeeIdAsync(long employeeSystemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get latest increment for an employee
    /// </summary>
    Task<SalaryIncrementLog?> GetLatestByEmployeeIdAsync(long employeeSystemId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all increment logs with pagination
    /// </summary>
    Task<IReadOnlyList<SalaryIncrementLog>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add new increment log
    /// </summary>
    Task<SalaryIncrementLog> AddAsync(SalaryIncrementLog log, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update increment log
    /// </summary>
    Task<SalaryIncrementLog> UpdateAsync(SalaryIncrementLog log, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get increments by status
    /// </summary>
    Task<IReadOnlyList<SalaryIncrementLog>> GetByStatusAsync(string status, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get increments within date range
    /// </summary>
    Task<IReadOnlyList<SalaryIncrementLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
}
