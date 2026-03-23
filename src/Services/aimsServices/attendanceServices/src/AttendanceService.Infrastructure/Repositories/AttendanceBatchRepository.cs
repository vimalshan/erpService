using AttendanceService.Domain.Entities;
using AttendanceService.Domain.Interfaces;
using AttendanceService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AttendanceService.Infrastructure.Repositories;

public class AttendanceBatchRepository(AppDbContext context)
    : Repository<AttendanceBatch>(context), IAttendanceBatchRepository
{
    public async Task<IEnumerable<AttendanceBatch>> GetByStatusAsync(string status, CancellationToken ct = default)
        => await DbSet.Where(x => x.BatchStatus.Value == status).ToListAsync(ct);

    public async Task<long> GetNextIdAsync(CancellationToken ct = default)
    {
        var max = await DbSet.MaxAsync(x => (long?)x.Id, ct);
        return (max ?? 0L) + 1L;
    }
}
