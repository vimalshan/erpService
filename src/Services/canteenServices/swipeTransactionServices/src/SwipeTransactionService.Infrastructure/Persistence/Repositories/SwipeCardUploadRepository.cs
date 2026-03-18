using Microsoft.EntityFrameworkCore;
using SwipeTransactionService.Domain.Entities;
using SwipeTransactionService.Domain.Interfaces.Repositories;

namespace SwipeTransactionService.Infrastructure.Persistence.Repositories;

public sealed class SwipeCardUploadRepository : ISwipeCardUploadRepository
{
    private readonly SwipeTransactionDbContext _context;

    public SwipeCardUploadRepository(SwipeTransactionDbContext context) => _context = context;

    public async Task<SwipeCardUpload?> GetBySerialNumberAsync(long serialNumber, CancellationToken ct = default)
        => await _context.SwipeCardUploads
            .AsNoTracking()
            .Where(x => x.SerialNumber == serialNumber)
            .FirstOrDefaultAsync(ct);

    public async Task<IEnumerable<SwipeCardUpload>> GetByEmployeeAsync(
        string employeeNumber, DateTime from, DateTime to, CancellationToken ct = default)
        => await _context.SwipeCardUploads
            .AsNoTracking()
            .Where(x => x.EmployeeNumber == employeeNumber && x.SwipeTime >= from && x.SwipeTime <= to)
            .ToListAsync(ct);

    public async Task<IEnumerable<SwipeCardUpload>> GetPendingAsync(CancellationToken ct = default)
        => await _context.SwipeCardUploads
            .AsNoTracking()
            .Where(x => x.UpdateStatus == 'P')
            .ToListAsync(ct);

    public async Task AddAsync(SwipeCardUpload entity, CancellationToken ct = default)
    {
        await _context.SwipeCardUploads.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(SwipeCardUpload entity, CancellationToken ct = default)
    {
        _context.SwipeCardUploads.Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<long> GetNextSerialNumberAsync(CancellationToken ct = default)
    {
        var max = await _context.SwipeCardUploads
            .Select(x => (long?)x.SerialNumber)
            .MaxAsync(ct);
        return (max ?? 0) + 1;
    }
}
