using Microsoft.EntityFrameworkCore;
using SwipeTransactionService.Domain.Entities;
using SwipeTransactionService.Domain.Interfaces.Repositories;

namespace SwipeTransactionService.Infrastructure.Persistence.Repositories;

public sealed class CanteenPunchRepository : ICanteenPunchRepository
{
    private readonly SwipeTransactionDbContext _context;

    public CanteenPunchRepository(SwipeTransactionDbContext context) => _context = context;

    public async Task<CanteenPunch?> GetByEmployeeAndDateAsync(long empSysId, DateTime date, CancellationToken ct = default)
        => await _context.CanteenPunches
            .AsNoTracking()
            .Where(x => x.EmployeeSysId == empSysId && x.PunchDate == date.Date)
            .FirstOrDefaultAsync(ct);

    public async Task<IEnumerable<CanteenPunch>> GetByEmployeeAsync(
        long empSysId, DateTime from, DateTime to, CancellationToken ct = default)
        => await _context.CanteenPunches
            .AsNoTracking()
            .Where(x => x.EmployeeSysId == empSysId && x.PunchDate >= from.Date && x.PunchDate <= to.Date)
            .ToListAsync(ct);

    public async Task AddAsync(CanteenPunch entity, CancellationToken ct = default)
    {
        await _context.CanteenPunches.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(CanteenPunch entity, CancellationToken ct = default)
    {
        _context.CanteenPunches.Update(entity);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<long> GetNextSerialNumberAsync(CancellationToken ct = default)
    {
        var max = await _context.CanteenPunches
            .Select(x => x.SerialNumber)
            .MaxAsync(ct);
        return (max ?? 0) + 1;
    }
}
