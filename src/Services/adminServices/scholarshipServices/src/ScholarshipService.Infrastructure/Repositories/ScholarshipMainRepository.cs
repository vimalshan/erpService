using Microsoft.EntityFrameworkCore;
using ScholarshipService.Domain.Entities;
using ScholarshipService.Domain.Repositories;
using ScholarshipService.Infrastructure.Data;

namespace ScholarshipService.Infrastructure.Repositories;

public class ScholarshipMainRepository(ScholarshipDbContext context) : IScholarshipMainRepository
{
    public async Task<ScholarshipMain?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await context.ScholarshipMains
            .Include(x => x.Details)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IEnumerable<ScholarshipMain>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.ScholarshipMains
            .Include(x => x.Details)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<IEnumerable<ScholarshipMain>> GetByEmployeeIdAsync(int employeeSysId, CancellationToken cancellationToken = default)
        => await context.ScholarshipMains
            .Include(x => x.Details)
            .Where(x => x.EmployeeSysId == employeeSysId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<int> GetNextIdAsync(CancellationToken cancellationToken = default)
    {
        var maxId = await context.ScholarshipMains.MaxAsync(x => (int?)x.Id, cancellationToken);
        return (maxId ?? 0) + 1;
    }

    public async Task AddAsync(ScholarshipMain scholarship, CancellationToken cancellationToken = default)
        => await context.ScholarshipMains.AddAsync(scholarship, cancellationToken);

    public Task UpdateAsync(ScholarshipMain scholarship, CancellationToken cancellationToken = default)
    {
        context.ScholarshipMains.Update(scholarship);
        return Task.CompletedTask;
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        => await context.ScholarshipMains.AnyAsync(x => x.Id == id, cancellationToken);
}
