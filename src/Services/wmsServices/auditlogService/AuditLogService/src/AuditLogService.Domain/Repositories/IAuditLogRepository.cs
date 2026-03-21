using AuditLogService.Domain.Entities;

namespace AuditLogService.Domain.Repositories;

public interface IAuditLogRepository
{
    Task<AuditLogEntry?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AuditLogEntry>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AuditLogEntry>> GetByTableNameAsync(string tableName, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AuditLogEntry>> GetByRecordIdAsync(string tableName, int recordId, CancellationToken cancellationToken = default);
    Task<AuditLogEntry> AddAsync(AuditLogEntry entity, CancellationToken cancellationToken = default);
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);
}
