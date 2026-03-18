using LovService.Domain.Entities;

namespace LovService.Domain.Interfaces;

public interface ILovMasterRepository
{
    Task<LovMaster?> GetByIdAsync(long lovId, CancellationToken ct = default);
    Task<IEnumerable<LovMaster>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<LovMaster>> GetByTypeIdAsync(int lovTypeId, CancellationToken ct = default);
    Task AddAsync(LovMaster entity, CancellationToken ct = default);
    Task UpdateAsync(LovMaster entity, CancellationToken ct = default);
    Task DeleteAsync(long lovId, CancellationToken ct = default);
    Task<bool> ExistsAsync(long lovId, CancellationToken ct = default);
    Task<long> GetNextIdAsync(CancellationToken ct = default);
}
