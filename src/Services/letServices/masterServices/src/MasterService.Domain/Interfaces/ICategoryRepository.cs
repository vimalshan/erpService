using MasterService.Domain.Entities;

namespace MasterService.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<Category?> GetByCodeAsync(string categoryCode, CancellationToken ct = default);
    Task<IEnumerable<Category>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(Category category, CancellationToken ct = default);
}

public interface IFinancialYearRepository
{
    Task<CompanyFinancialYear?> GetBySerialAsync(long serial, CancellationToken ct = default);
    Task<IEnumerable<CompanyFinancialYear>> GetActiveAsync(CancellationToken ct = default);
    Task AddAsync(CompanyFinancialYear fy, CancellationToken ct = default);
    Task UpdateAsync(CompanyFinancialYear fy, CancellationToken ct = default);
}
