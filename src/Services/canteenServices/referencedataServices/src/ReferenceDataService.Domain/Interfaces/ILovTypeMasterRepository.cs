using ReferenceDataService.Domain.Entities;

namespace ReferenceDataService.Domain.Interfaces;

public interface ILovTypeMasterRepository
{
    Task<LovTypeMaster?> GetByCodeAsync(string lovTypeCode, CancellationToken cancellationToken = default);
    Task<IEnumerable<LovTypeMaster>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(LovTypeMaster entity, CancellationToken cancellationToken = default);
    void Update(LovTypeMaster entity);
    void Delete(LovTypeMaster entity);
}
