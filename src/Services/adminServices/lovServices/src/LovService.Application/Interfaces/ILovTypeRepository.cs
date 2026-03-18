using LovService.Domain.Entities;

namespace LovService.Application.Interfaces;

public interface ILovTypeRepository
{
    Task<LovType?> GetByIdAsync(long id, CancellationToken ct = default);
    Task<IEnumerable<LovType>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(LovType lovType, CancellationToken ct = default);
    void Update(LovType lovType);
    void Delete(LovType lovType);
    Task<bool> ExistsAsync(long id, CancellationToken ct = default);
}
