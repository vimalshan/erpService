using Microsoft.EntityFrameworkCore;
using TransactionService.Domain.Entities;
using TransactionService.Domain.Interfaces;
using TransactionService.Infrastructure.Persistence;

namespace TransactionService.Infrastructure.Repositories;

public class TransactionLogRepository : ITransactionLogRepository
{
    private readonly TransactionDbContext _context;

    public TransactionLogRepository(TransactionDbContext context) => _context = context;

    public async Task<TransactionLog?> GetByIdAsync(long id, CancellationToken cancellationToken = default) =>
        await _context.TransactionLogs.FindAsync([id], cancellationToken);

    public async Task<IReadOnlyList<TransactionLog>> GetByEntityAsync(string transactionType, long transactionId, CancellationToken cancellationToken = default) =>
        await _context.TransactionLogs
            .Where(l => l.TransactionType == transactionType && l.TransactionId == transactionId)
            .OrderByDescending(l => l.CreatedOn)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<TransactionLog>> GetByActionAsync(string action, CancellationToken cancellationToken = default) =>
        await _context.TransactionLogs
            .Where(l => l.Action == action)
            .OrderByDescending(l => l.CreatedOn)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<TransactionLog>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _context.TransactionLogs
            .OrderByDescending(l => l.CreatedOn)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public IQueryable<TransactionLog> GetQueryable() =>
        _context.TransactionLogs.AsNoTracking();

    public async Task AddAsync(TransactionLog log, CancellationToken cancellationToken = default) =>
        await _context.TransactionLogs.AddAsync(log, cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);
}
