using UtilityService.Domain.Entities;

namespace UtilityService.Domain.Interfaces;

public interface IToadPlanSqlRepository
{
    Task<ToadPlanSql?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ToadPlanSql?> GetByStatementIdAsync(string statementId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ToadPlanSql>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ToadPlanSql>> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<IEnumerable<ToadPlanSql>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);
    Task AddAsync(ToadPlanSql entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(ToadPlanSql entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string statementId, CancellationToken cancellationToken = default);
}
