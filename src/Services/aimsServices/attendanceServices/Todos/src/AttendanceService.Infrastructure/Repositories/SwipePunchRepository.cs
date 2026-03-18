using AttendanceService.Domain.Entities;
using AttendanceService.Domain.Interfaces;
using AttendanceService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AttendanceService.Infrastructure.Repositories;

public class SwipePunchRepository(AppDbContext context)
    : Repository<SwipeRawPunch>(context), ISwipePunchRepository
{
    public async Task<IEnumerable<SwipeRawPunch>> GetByEmployeeAsync(long empSysId, CancellationToken ct = default)
        => await DbSet.Where(x => x.SwipeEmpSysId == empSysId).OrderByDescending(x => x.SwipePunchTime).ToListAsync(ct);

    public async Task<IEnumerable<SwipeRawPunch>> GetByEmployeeAndDateRangeAsync(
        long empSysId, DateTime from, DateTime to, CancellationToken ct = default)
        => await DbSet
            .Where(x => x.SwipeEmpSysId == empSysId
                && x.SwipePunchTime >= from && x.SwipePunchTime <= to)
            .OrderBy(x => x.SwipePunchTime)
            .ToListAsync(ct);

    public async Task<int> GetDistinctPunchDaysAsync(
        long empSysId, DateTime from, DateTime to, CancellationToken ct = default)
        => await DbSet
            .Where(x => x.SwipeEmpSysId == empSysId
                && x.SwipePunchTime >= from && x.SwipePunchTime <= to)
            .Select(x => x.SwipePunchTime.Date)
            .Distinct()
            .CountAsync(ct);

    public async Task<long> GetNextIdAsync(CancellationToken ct = default)
    {
        var max = await DbSet.MaxAsync(x => (long?)x.Id, ct);
        return (max ?? 0L) + 1L;
    }
}
