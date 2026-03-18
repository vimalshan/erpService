using EmployeeService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EmployeeService.Domain.Repositories
{
    /// <summary>
    /// Unit of Work Pattern - Coordinates work across multiple repositories
    /// </summary>
    public interface IUnitOfWork : IAsyncDisposable
    {
        IEmployeeRepository Employees { get; }
        IRepository<EmployeeAppraisal> Appraisals { get; }
        IRepository<EmployeeCareerPlan> CareerPlans { get; }
        IRepository<EmployeeBenefit> Benefits { get; }
        IRepository<EmployeeAccountability> Accountabilities { get; }

        Task<int> SaveChangesAsync();
        Task<bool> BeginTransactionAsync();
        Task<bool> CommitTransactionAsync();
        Task<bool> RollbackTransactionAsync();
    }

    /// <summary>
    /// Generic Repository Interface
    /// </summary>
    public interface IRepository<T> where T : class
    {
        Task<T> GetByIdAsync(long id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);
        Task AddAsync(T entity);
        Task AddRangeAsync(IEnumerable<T> entities);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        Task<bool> SaveChangesAsync();
    }

    /// <summary>
    /// Employee specific repository interface
    /// </summary>
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee> GetEmployeeWithAllDetailsAsync(long employeeId);
        Task<Employee> GetByEmployeeNumberAsync(string employeeNumber);
        Task<Employee> GetByUserIdAsync(string userId);
        Task<IEnumerable<Employee>> GetActiveEmployeesAsync();
        Task<IEnumerable<Employee>> GetEmployeesByUnitAsync(long unitId);
        Task<IEnumerable<Employee>> GetEmployeesByGradeAsync(string gradeCode);
        Task<IEnumerable<Employee>> GetEmployeesByDesignationAsync(string designation);
        Task<IEnumerable<Employee>> GetTerminatedEmployeesAsync();
        Task<bool> EmployeeExistsAsync(long employeeId);
        Task<bool> IsEmployeeNumberUniqueAsync(string employeeNumber, long? excludeId = null);
        Task<bool> IsUserIdUniqueAsync(string userId, long? excludeId = null);
        Task<int> GetTotalEmployeeCountAsync();
        Task<int> GetActiveEmployeeCountAsync();
        Task<decimal> GetAverageSalaryByGradeAsync(string gradeCode);
        Task<IEnumerable<Employee>> SearchEmployeesAsync(string searchTerm);
        Task<IEnumerable<Employee>> GetEmployeesWithAppraisalDueAsync(long financialYearId);
        Task<IEnumerable<Employee>> GetEmployeesForCareerPlanningAsync();
        Task<IEnumerable<Employee>> GetEmployeesByReportingManagerAsync(long managerEmployeeId);
    }

    /// <summary>
    /// Repository for Appraisals
    /// </summary>
    public interface IAppraisalRepository : IRepository<EmployeeAppraisal>
    {
        Task<IEnumerable<EmployeeAppraisal>> GetAppraisalsByEmployeeAsync(long employeeId);
        Task<IEnumerable<EmployeeAppraisal>> GetAppraisalsByFinancialYearAsync(long financialYearId);
        Task<IEnumerable<EmployeeAppraisal>> GetPendingAppraisalsAsync(long appraiserEmployeeId);
        Task<EmployeeAppraisal> GetAppraisalWithDetailsAsync(long appraisalId);
        Task<IEnumerable<EmployeeAppraisal>> GetAppraisalsByStatusAsync(string status);
    }

    /// <summary>
    /// Repository for Career Plans
    /// </summary>
    public interface ICareerPlanRepository : IRepository<EmployeeCareerPlan>
    {
        Task<IEnumerable<EmployeeCareerPlan>> GetCareerPlansByEmployeeAsync(long employeeId);
        Task<IEnumerable<EmployeeCareerPlan>> GetCareerPlansByStatusAsync(string status);
        Task<EmployeeCareerPlan> GetLatestCareerPlanAsync(long employeeId);
    }

    /// <summary>
    /// Repository for Benefits
    /// </summary>
    public interface IBenefitRepository : IRepository<EmployeeBenefit>
    {
        Task<IEnumerable<EmployeeBenefit>> GetBenefitsByEmployeeAsync(long employeeId);
        Task<IEnumerable<EmployeeBenefit>> GetBenefitsByFinancialYearAsync(long financialYearId);
        Task<IEnumerable<EmployeeBenefit>> GetActiveBenefitsAsync();
        Task<decimal> GetTotalBenefitsForEmployeeAsync(long employeeId, long financialYearId);
    }

    /// <summary>
    /// Repository for Accountabilities
    /// </summary>
    public interface IAccountabilityRepository : IRepository<EmployeeAccountability>
    {
        Task<IEnumerable<EmployeeAccountability>> GetAccountabilitiesByEmployeeAsync(long employeeId);
        Task<IEnumerable<EmployeeAccountability>> GetOpenAccountabilitiesAsync();
        Task<IEnumerable<EmployeeAccountability>> GetAccountabilitiesByPositionAsync(long positionId);
    }
}
