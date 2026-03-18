using Masters.Domain.Entities;

namespace Masters.Application.Interfaces;

public interface ILovMasterRepository
{
    Task<LovMaster?> GetByIdAsync(long lovId, CancellationToken cancellationToken = default);
    Task<IEnumerable<LovMaster>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<LovMaster>> GetByTypeAsync(string lovType, CancellationToken cancellationToken = default);
    Task<LovMaster> AddAsync(LovMaster entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(LovMaster entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(long lovId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(long lovId, CancellationToken cancellationToken = default);
}
