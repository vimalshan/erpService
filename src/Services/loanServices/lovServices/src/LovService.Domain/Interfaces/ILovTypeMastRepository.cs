using LovService.Domain.Entities;

namespace LovService.Domain.Interfaces;

public interface ILovTypeMastRepository
{
    Task<LovTypeMast?> GetByIdAsync(int lovTypeId, CancellationToken ct = default);
    Task<IEnumerable<LovTypeMast>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<LovTypeMast>> GetByOrgIdAsync(int orgId, CancellationToken ct = default);
    Task AddAsync(LovTypeMast entity, CancellationToken ct = default);
    Task UpdateAsync(LovTypeMast entity, CancellationToken ct = default);
    Task DeleteAsync(int lovTypeId, CancellationToken ct = default);
    Task<bool> ExistsAsync(int lovTypeId, CancellationToken ct = default);
}
