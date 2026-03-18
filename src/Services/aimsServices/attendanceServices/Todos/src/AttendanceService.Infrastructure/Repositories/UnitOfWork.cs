using AttendanceService.Domain.Interfaces;
using AttendanceService.Infrastructure.Persistence;

namespace AttendanceService.Infrastructure.Repositories;

public class UnitOfWork(AppDbContext context,
    ISwipePunchRepository swipePunches,
    IAttendanceBatchRepository attendanceBatches) : IUnitOfWork
{
    public ISwipePunchRepository SwipePunches => swipePunches;
    public IAttendanceBatchRepository AttendanceBatches => attendanceBatches;

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await context.SaveChangesAsync(ct);
}
