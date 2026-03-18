using Microsoft.EntityFrameworkCore;
using SwipeTransactionService.Domain.Entities;
using SwipeTransactionService.Domain.Interfaces.Repositories;

namespace SwipeTransactionService.Infrastructure.Persistence.Repositories;

public sealed class DailyAvailedRepository : IDailyAvailedRepository
{
    private readonly SwipeTransactionDbContext _context;

    public DailyAvailedRepository(SwipeTransactionDbContext context) => _context = context;

    public async Task<DailyAvailed?> GetBySerialAsync(long serialNumber, CancellationToken ct = default)
        => await _context.DailyAvaileds.FindAsync(new object[] { serialNumber }, ct);

    public async Task<IEnumerable<DailyAvailed>> GetByEmployeeAsync(
        long empSysId, string date, CancellationToken ct = default)
        => await _context.DailyAvaileds
            .AsNoTracking()
            .Where(x => x.EmployeeSysId == empSysId && x.SwipeDate == date)
            .ToListAsync(ct);

    public async Task AddAsync(DailyAvailed entity, CancellationToken ct = default)
    {
        await _context.DailyAvaileds.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<long> GetNextSerialNumberAsync(CancellationToken ct = default)
    {
        var max = await _context.DailyAvaileds
            .Select(x => (long?)x.SerialNumber)
            .MaxAsync(ct);
        return (max ?? 0) + 1;
    }
}
