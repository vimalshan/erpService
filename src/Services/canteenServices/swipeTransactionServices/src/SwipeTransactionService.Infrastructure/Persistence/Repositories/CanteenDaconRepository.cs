using Microsoft.EntityFrameworkCore;
using SwipeTransactionService.Domain.Entities;
using SwipeTransactionService.Domain.Interfaces.Repositories;

namespace SwipeTransactionService.Infrastructure.Persistence.Repositories;

public sealed class CanteenDaconRepository : ICanteenDaconRepository
{
    private readonly SwipeTransactionDbContext _context;

    public CanteenDaconRepository(SwipeTransactionDbContext context) => _context = context;

    public async Task<IEnumerable<CanteenDacon>> GetByEmployeeAsync(
        long empSysId, string date, CancellationToken ct = default)
        => await _context.CanteenDacons
            .AsNoTracking()
            .Where(x => x.EmployeeSysId == empSysId && x.SwipeDate == date)
            .ToListAsync(ct);

    public async Task AddAsync(CanteenDacon entity, CancellationToken ct = default)
    {
        await _context.CanteenDacons.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<long> GetNextSerialNumberAsync(CancellationToken ct = default)
    {
        var max = await _context.CanteenDacons
            .Select(x => x.SerialNumber)
            .MaxAsync(ct);
        return (max ?? 0) + 1;
    }
}
