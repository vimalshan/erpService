using ReferenceDataService.Domain.Entities;

namespace ReferenceDataService.Domain.Interfaces;

public interface ILovMasterRepository
{
    Task<LovMaster?> GetByIdAsync(string lovId, CancellationToken cancellationToken = default);
    Task<IEnumerable<LovMaster>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<LovMaster>> GetByTypeAsync(string lovType, CancellationToken cancellationToken = default);
    Task AddAsync(LovMaster entity, CancellationToken cancellationToken = default);
    void Update(LovMaster entity);
    void Delete(LovMaster entity);
}
