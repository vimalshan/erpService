using ReferenceDataService.Domain.Entities;

namespace ReferenceDataService.Domain.Interfaces;

public interface IPathToSqlServerRepository
{
    Task<PathToSqlServer?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<PathToSqlServer>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<PathToSqlServer?> GetByCompanyCodeAsync(string companyCode, CancellationToken cancellationToken = default);
    Task AddAsync(PathToSqlServer entity, CancellationToken cancellationToken = default);
    void Update(PathToSqlServer entity);
    void Delete(PathToSqlServer entity);
}
