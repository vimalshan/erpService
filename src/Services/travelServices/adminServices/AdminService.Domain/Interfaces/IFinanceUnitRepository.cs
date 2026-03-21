using AdminService.Domain.Entities;

namespace AdminService.Domain.Interfaces;

/// <summary>
/// Repository interface for FinanceUnit entity
/// </summary>
public interface IFinanceUnitRepository
{
    Task<FinanceUnit?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<FinanceUnit?> GetByUnitIdAsync(long unitId, CancellationToken cancellationToken = default);
    Task<IEnumerable<FinanceUnit>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<FinanceUnit> AddAsync(FinanceUnit financeUnit, CancellationToken cancellationToken = default);
    Task<FinanceUnit> UpdateAsync(FinanceUnit financeUnit, CancellationToken cancellationToken = default);
    Task DeleteAsync(long id, CancellationToken cancellationToken = default);
}
