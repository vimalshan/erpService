using FindingsAPI.Gateway.Domain.Entities;

namespace FindingsAPI.Gateway.Domain.Interfaces;

public interface IFindingsDomainRepository
{
    Task<FindingEntity?> GetByIdAsync(int findingId);
    Task<IEnumerable<FindingEntity>> GetAllAsync();
    Task<IEnumerable<FindingEntity>> GetByAuditAsync(int auditId);
    Task<IEnumerable<FindingEntity>> GetBySiteAsync(int siteId);
    Task<FindingEntity> AddAsync(FindingEntity entity);
    Task UpdateAsync(FindingEntity entity);
    Task DeleteAsync(int findingId);
    Task<IEnumerable<FindingStatusEntity>> GetStatusesAsync();
    Task<IEnumerable<FindingCategoryEntity>> GetCategoriesAsync();
    Task<FindingResponseEntity> AddResponseAsync(FindingResponseEntity response);
    Task<IEnumerable<FindingResponseEntity>> GetResponsesByFindingAsync(int findingId);
}
