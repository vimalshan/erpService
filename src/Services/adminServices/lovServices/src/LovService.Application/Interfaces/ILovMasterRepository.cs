using LovService.Domain.Entities;

namespace LovService.Application.Interfaces;

public interface ILovMasterRepository
{
    Task<LovMaster?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<LovMaster>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<LovMaster>> GetByTypeIdAsync(long lovTypeId, CancellationToken ct = default);
    Task AddAsync(LovMaster lovMaster, CancellationToken ct = default);
    void Update(LovMaster lovMaster);
    void Delete(LovMaster lovMaster);
    Task<bool> ExistsAsync(long id, CancellationToken ct = default);
}
