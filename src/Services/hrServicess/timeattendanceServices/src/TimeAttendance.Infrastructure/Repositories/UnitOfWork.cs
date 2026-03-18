using TimeAttendance.Domain.Interfaces;
using TimeAttendance.Infrastructure.Persistence;

namespace TimeAttendance.Infrastructure.Repositories;

public class UnitOfWork(
    TimeAttendanceDbContext context,
    IAbsenteeismDetailRepository absenteeismDetails,
    IAbsenteeismMisRepository absenteeismMis) : IUnitOfWork
{
    public IAbsenteeismDetailRepository AbsenteeismDetails { get; } = absenteeismDetails;
    public IAbsenteeismMisRepository AbsenteeismMis { get; } = absenteeismMis;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => context.SaveChangesAsync(cancellationToken);

    public void Dispose() => context.Dispose();
}
