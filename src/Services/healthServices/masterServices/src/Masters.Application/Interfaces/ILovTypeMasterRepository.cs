using Masters.Domain.Entities;

namespace Masters.Application.Interfaces;

public interface ILovTypeMasterRepository
{
    Task<LovTypeMaster?> GetByIdAsync(string lovTypeCode, CancellationToken cancellationToken = default);
    Task<IEnumerable<LovTypeMaster>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<LovTypeMaster> AddAsync(LovTypeMaster entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(LovTypeMaster entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(string lovTypeCode, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string lovTypeCode, CancellationToken cancellationToken = default);
}
