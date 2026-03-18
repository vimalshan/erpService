using FinyearAPI.Domain.Entities;

namespace FinyearAPI.Repositories.Interfaces
{
    /// <summary>
    /// Generic Repository interface with common CRUD operations
    /// </summary>
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(long id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<bool> DeleteAsync(long id);
        Task<bool> ExistsAsync(long id);
    }

    /// <summary>
    /// Repository interface for FinancialYearMaster entity
    /// Defines business-specific query operations
    /// </summary>
    public interface IFinancialYearRepository : IRepository<FinancialYearMaster>
    {
        Task<FinancialYearMaster?> GetCurrentFinancialYearAsync();
        Task<FinancialYearMaster?> GetByNameAsync(string financialYearName);
        Task<IEnumerable<FinancialYearMaster>> GetActiveFinancialYearsAsync();
        Task<IEnumerable<FinancialYearMaster>> GetFinancialYearsByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
