using FinyearAPI.Application.DTOs;
using FinyearAPI.Domain.Entities;

namespace FinyearAPI.Application.Services
{
    /// <summary>
    /// Financial Year Service Interface
    /// Defines business logic operations for financial year management
    /// Moved to Application layer to avoid circular dependencies with GraphQL
    /// </summary>
    public interface IFinancialYearService
    {
        Task<FinancialYearMaster?> GetFinancialYearByIdAsync(long id);
        Task<IEnumerable<FinancialYearMaster>> GetAllFinancialYearsAsync();
        Task<FinancialYearMaster?> GetCurrentFinancialYearAsync();
        Task<FinancialYearMaster?> GetFinancialYearByNameAsync(string name);
        Task<FinancialYearMaster> CreateFinancialYearAsync(CreateFinancialYearDto dto);
        Task<FinancialYearMaster> UpdateFinancialYearAsync(long id, UpdateFinancialYearDto dto);
        Task<bool> DeleteFinancialYearAsync(long id);
    }
}
