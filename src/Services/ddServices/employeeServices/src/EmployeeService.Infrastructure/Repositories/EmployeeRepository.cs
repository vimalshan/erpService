using EmployeeService.Domain.Entities;
using EmployeeService.Domain.Repositories;
using EmployeeService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeService.Infrastructure.Repositories
{
    /// <summary>
    /// Employee Repository - specific implementation for Employee aggregate
    /// </summary>
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly EmployeeServiceDbContext _employeeContext;

        public EmployeeRepository(EmployeeServiceDbContext context) : base(context)
        {
            _employeeContext = context;
        }

        public async Task<Employee> GetEmployeeWithAllDetailsAsync(long employeeId)
        {
            return await _employeeContext.Employees
                .Include(e => e.Accountabilities)
                .Include(e => e.Appraisals)
                    .ThenInclude(a => a.Objectives)
                .Include(e => e.Appraisals)
                    .ThenInclude(a => a.Competencies)
                .Include(e => e.CareerPlans)
                .Include(e => e.Benefits)
                .FirstOrDefaultAsync(e => e.Id == employeeId);
        }

        public async Task<Employee> GetByEmployeeNumberAsync(string employeeNumber)
        {
            return await _employeeContext.Employees
                .FirstOrDefaultAsync(e => e.EmploymentDetails.EmployeeNumber == employeeNumber);
        }

        public async Task<Employee> GetByUserIdAsync(string userId)
        {
            return await _employeeContext.Employees
                .FirstOrDefaultAsync(e => e.EmploymentDetails.UserId == userId);
        }

        public async Task<IEnumerable<Employee>> GetActiveEmployeesAsync()
        {
            return await _employeeContext.Employees
                .Where(e => e.Status == "ACTIVE" && !e.IsTerminated)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByUnitAsync(long unitId)
        {
            return await _employeeContext.Employees
                .Where(e => e.OrganizationalAssignment.UnitOrgId == unitId && !e.IsTerminated)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByGradeAsync(string gradeCode)
        {
            return await _employeeContext.Employees
                .Where(e => e.GradeInfo.GradeCode == gradeCode && !e.IsTerminated)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByDesignationAsync(string designation)
        {
            return await _employeeContext.Employees
                .Where(e => e.OrganizationalAssignment.Designation == designation && !e.IsTerminated)
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetTerminatedEmployeesAsync()
        {
            return await _employeeContext.Employees
                .IgnoreQueryFilters() // Ignore soft delete filter
                .Where(e => e.IsTerminated)
                .ToListAsync();
        }

        public async Task<bool> EmployeeExistsAsync(long employeeId)
        {
            return await _employeeContext.Employees
                .AnyAsync(e => e.Id == employeeId);
        }

        public async Task<bool> IsEmployeeNumberUniqueAsync(string employeeNumber, long? excludeId = null)
        {
            var query = _employeeContext.Employees
                .Where(e => e.EmploymentDetails.EmployeeNumber == employeeNumber);

            if (excludeId.HasValue)
                query = query.Where(e => e.Id != excludeId.Value);

            return !(await query.AnyAsync());
        }

        public async Task<bool> IsUserIdUniqueAsync(string userId, long? excludeId = null)
        {
            var query = _employeeContext.Employees
                .Where(e => e.EmploymentDetails.UserId == userId);

            if (excludeId.HasValue)
                query = query.Where(e => e.Id != excludeId.Value);

            return !(await query.AnyAsync());
        }

        public async Task<int> GetTotalEmployeeCountAsync()
        {
            return await _employeeContext.Employees.CountAsync();
        }

        public async Task<int> GetActiveEmployeeCountAsync()
        {
            return await _employeeContext.Employees
                .Where(e => e.Status == "ACTIVE" && !e.IsTerminated)
                .CountAsync();
        }

        public async Task<decimal> GetAverageSalaryByGradeAsync(string gradeCode)
        {
            var employees = await _employeeContext.Employees
                .Where(e => e.GradeInfo.GradeCode == gradeCode && !e.IsTerminated)
                .ToListAsync();

            if (!employees.Any())
                return 0;

            return employees.Average(e => e.SalaryInfo.BasicSalary);
        }

        public async Task<IEnumerable<Employee>> SearchEmployeesAsync(string searchTerm)
        {
            var lowerSearchTerm = searchTerm.ToLower();
            return await _employeeContext.Employees
                .Where(e => 
                    e.PersonalInfo.FirstName.ToLower().Contains(lowerSearchTerm) ||
                    e.PersonalInfo.LastName.ToLower().Contains(lowerSearchTerm) ||
                    e.EmploymentDetails.EmployeeNumber.ToLower().Contains(lowerSearchTerm) ||
                    e.EmploymentDetails.UserId.ToLower().Contains(lowerSearchTerm) ||
                    e.ContactInfo.Email.ToLower().Contains(lowerSearchTerm) ||
                    e.OrganizationalAssignment.Designation.ToLower().Contains(lowerSearchTerm))
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesWithAppraisalDueAsync(long financialYearId)
        {
            return await _employeeContext.Employees
                .Where(e => !e.IsTerminated &&
                    !e.Appraisals.Any(a => a.FinancialYearId == financialYearId && 
                        (a.Status == "APPROVED" || a.Status == "SUBMITTED")))
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesForCareerPlanningAsync()
        {
            return await _employeeContext.Employees
                .Where(e => !e.IsTerminated && e.Status == "ACTIVE")
                .ToListAsync();
        }

        public async Task<IEnumerable<Employee>> GetEmployeesByReportingManagerAsync(long managerEmployeeId)
        {
            // This would typically join with a separate ReportingManager table or hierarchy table
            // For now, returning empty as the schema doesn't have explicit manager relationship
            return await Task.FromResult(new List<Employee>());
        }
    }
}
