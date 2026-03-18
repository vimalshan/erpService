using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EmployeeService.Domain.Entities;
using EmployeeService.Domain.Repositories;

namespace EmployeeService.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of IEmployeeRepository
/// </summary>
public class EmployeeRepository : IEmployeeRepository
{
    private readonly Persistence.EmployeeDbContext _context;

    public EmployeeRepository(Persistence.EmployeeDbContext context)
    {
        _context = context;
    }

    public async Task<Employee?> GetBySystemIdAsync(long employeeSystemId, CancellationToken cancellationToken = default)
    {
        return await _context.Employees
            .FirstOrDefaultAsync(e => e.EmployeeSystemId == employeeSystemId && !e.IsDeleted, cancellationToken);
    }

    public async Task<Employee?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.Employees
            .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<Employee>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Employees
            .Where(e => !e.IsDeleted && e.EmploymentStatus == "Active")
            .OrderBy(e => e.FirstName)
            .ThenBy(e => e.LastName)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Employee>> GetByCostCenterAsync(string costCenterId, CancellationToken cancellationToken = default)
    {
        return await _context.Employees
            .Where(e => e.CostCenterId == costCenterId && !e.IsDeleted && e.EmploymentStatus == "Active")
            .OrderBy(e => e.FirstName)
            .ThenBy(e => e.LastName)
            .ToListAsync(cancellationToken);
    }

    public async Task<Employee> AddAsync(Employee employee, CancellationToken cancellationToken = default)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync(cancellationToken);
        return employee;
    }

    public async Task<Employee> UpdateAsync(Employee employee, CancellationToken cancellationToken = default)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync(cancellationToken);
        return employee;
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken cancellationToken = default)
    {
        var employee = await GetByIdAsync(id, cancellationToken);
        if (employee == null)
            return false;

        employee.IsDeleted = true;
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> ExistsAsync(long employeeSystemId, CancellationToken cancellationToken = default)
    {
        return await _context.Employees
            .AnyAsync(e => e.EmployeeSystemId == employeeSystemId && !e.IsDeleted, cancellationToken);
    }
}

/// <summary>
/// EF Core implementation of ISalaryIncrementLogRepository
/// </summary>
public class SalaryIncrementLogRepository : ISalaryIncrementLogRepository
{
    private readonly Persistence.EmployeeDbContext _context;

    public SalaryIncrementLogRepository(Persistence.EmployeeDbContext context)
    {
        _context = context;
    }

    public async Task<SalaryIncrementLog?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.SalaryIncrementLogs
            .FirstOrDefaultAsync(l => l.Id == id && !l.IsDeleted, cancellationToken);
    }

    public async Task<IReadOnlyList<SalaryIncrementLog>> GetByEmployeeIdAsync(long employeeSystemId, CancellationToken cancellationToken = default)
    {
        return await _context.SalaryIncrementLogs
            .Where(l => l.EmployeeSystemId == employeeSystemId && !l.IsDeleted)
            .OrderByDescending(l => l.EffectiveDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<SalaryIncrementLog?> GetLatestByEmployeeIdAsync(long employeeSystemId, CancellationToken cancellationToken = default)
    {
        return await _context.SalaryIncrementLogs
            .Where(l => l.EmployeeSystemId == employeeSystemId && !l.IsDeleted)
            .OrderByDescending(l => l.EffectiveDate)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SalaryIncrementLog>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
    {
        return await _context.SalaryIncrementLogs
            .Where(l => !l.IsDeleted)
            .OrderByDescending(l => l.ApprovedOn)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<SalaryIncrementLog> AddAsync(SalaryIncrementLog log, CancellationToken cancellationToken = default)
    {
        _context.SalaryIncrementLogs.Add(log);
        await _context.SaveChangesAsync(cancellationToken);
        return log;
    }

    public async Task<SalaryIncrementLog> UpdateAsync(SalaryIncrementLog log, CancellationToken cancellationToken = default)
    {
        _context.SalaryIncrementLogs.Update(log);
        await _context.SaveChangesAsync(cancellationToken);
        return log;
    }

    public async Task<IReadOnlyList<SalaryIncrementLog>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        return await _context.SalaryIncrementLogs
            .Where(l => l.Status == status && !l.IsDeleted)
            .OrderByDescending(l => l.ApprovedOn)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SalaryIncrementLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await _context.SalaryIncrementLogs
            .Where(l => l.EffectiveDate >= startDate && l.EffectiveDate <= endDate && !l.IsDeleted)
            .OrderBy(l => l.EffectiveDate)
            .ToListAsync(cancellationToken);
    }
}
