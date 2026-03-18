using Microsoft.EntityFrameworkCore;
using TimeAttendance.Domain.Entities;
using TimeAttendance.Domain.Interfaces;

namespace TimeAttendance.Infrastructure.Repositories;

public class AbsenteeismDetailRepository(TimeAttendance.Infrastructure.Persistence.TimeAttendanceDbContext context)
    : IAbsenteeismDetailRepository
{
    public async Task<AbsenteeismDetail?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => await context.AbsenteeismDetails
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IEnumerable<AbsenteeismDetail>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.AbsenteeismDetails.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<IEnumerable<AbsenteeismDetail>> GetByUnitAndPeriodAsync(
        long unitId, int year, int month, CancellationToken cancellationToken = default)
        => await context.AbsenteeismDetails
            .AsNoTracking()
            .Where(x => x.UnitId == unitId && x.Year == year && x.Month == month)
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<AbsenteeismDetail>> GetByUnitAsync(
        long unitId, CancellationToken cancellationToken = default)
        => await context.AbsenteeismDetails
            .AsNoTracking()
            .Where(x => x.UnitId == unitId)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(AbsenteeismDetail entity, CancellationToken cancellationToken = default)
        => await context.AbsenteeismDetails.AddAsync(entity, cancellationToken);

    public void Update(AbsenteeismDetail entity)
        => context.AbsenteeismDetails.Update(entity);

    public void Remove(AbsenteeismDetail entity)
        => context.AbsenteeismDetails.Remove(entity);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => context.SaveChangesAsync(cancellationToken);
}
