using ErrorLoggingService.Domain.Entities;
using ErrorLoggingService.Domain.Repositories;
using ErrorLoggingService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ErrorLoggingService.Infrastructure.Repositories;

public sealed class ErrorLogRepository : IErrorLogRepository
{
    private readonly AppDbContext _context;

    public ErrorLogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ErrorLog?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await _context.ErrorLogs.FindAsync(new object[] { id }, cancellationToken);

    public async Task<IEnumerable<ErrorLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        => await _context.ErrorLogs
            .Where(e => e.ErrorDate >= startDate && e.ErrorDate <= endDate)
            .OrderByDescending(e => e.ErrorDate)
            .Take(1000)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(ErrorLog errorLog, CancellationToken cancellationToken = default)
        => await _context.ErrorLogs.AddAsync(errorLog, cancellationToken);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}
