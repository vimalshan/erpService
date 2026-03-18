using Microsoft.EntityFrameworkCore;
using TimeAttendance.Domain.Entities;
using TimeAttendance.Domain.Interfaces;

namespace TimeAttendance.Infrastructure.Repositories;

public class AbsenteeismMisRepository(TimeAttendance.Infrastructure.Persistence.TimeAttendanceDbContext context)
    : IAbsenteeismMisRepository
{
    public async Task<AbsenteeismMis?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => await context.AbsenteeismMisRecords
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IEnumerable<AbsenteeismMis>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.AbsenteeismMisRecords.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<IEnumerable<AbsenteeismMis>> GetByUnitAndMonthAsync(
        int unitId, string month, CancellationToken cancellationToken = default)
        => await context.AbsenteeismMisRecords
            .AsNoTracking()
            .Where(x => x.UnitId == unitId && x.Month == month)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(AbsenteeismMis entity, CancellationToken cancellationToken = default)
        => await context.AbsenteeismMisRecords.AddAsync(entity, cancellationToken);

    public void Update(AbsenteeismMis entity)
        => context.AbsenteeismMisRecords.Update(entity);

    public void Remove(AbsenteeismMis entity)
        => context.AbsenteeismMisRecords.Remove(entity);

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => context.SaveChangesAsync(cancellationToken);
}
