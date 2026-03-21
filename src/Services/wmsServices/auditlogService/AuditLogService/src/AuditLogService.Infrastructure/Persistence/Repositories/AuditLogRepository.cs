using AuditLogService.Domain.Entities;
using AuditLogService.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AuditLogService.Infrastructure.Persistence.Repositories;

public class AuditLogRepository : IAuditLogRepository
{
    private readonly AuditLogDbContext _context;

    public AuditLogRepository(AuditLogDbContext context)
    {
        _context = context;
    }

    public async Task<AuditLogEntry?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        return await _context.AuditLogs.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<AuditLogEntry>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.AuditLogs
            .OrderByDescending(a => a.ChangeDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AuditLogEntry>> GetByTableNameAsync(string tableName, CancellationToken cancellationToken = default)
    {
        return await _context.AuditLogs
            .Where(a => a.TableName == tableName)
            .OrderByDescending(a => a.ChangeDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AuditLogEntry>> GetByRecordIdAsync(string tableName, int recordId, CancellationToken cancellationToken = default)
    {
        return await _context.AuditLogs
            .Where(a => a.TableName == tableName && a.RecordId == recordId)
            .OrderByDescending(a => a.ChangeDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<AuditLogEntry> AddAsync(AuditLogEntry entity, CancellationToken cancellationToken = default)
    {
        await _context.AuditLogs.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.AuditLogs.CountAsync(cancellationToken);
    }
}
