using MasterService.Domain.Entities;
using MasterService.Domain.Interfaces;
using MasterService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MasterService.Infrastructure.Repositories;

public sealed class JobRepository(ApplicationDbContext context) : IJobRepository
{
    public async Task<JobMaster?> GetByCodeAsync(long jobCode, CancellationToken ct = default)
        => await context.JobMasters.FindAsync([jobCode], ct);

    public async Task<IEnumerable<JobMaster>> GetByCategoryAsync(string? categoryCode = null, CancellationToken ct = default)
    {
        var query = context.JobMasters.AsQueryable();
        if (!string.IsNullOrWhiteSpace(categoryCode))
            query = query.Where(j => j.CategoryCode == categoryCode.ToUpper());
        return await query.OrderBy(j => j.JobName).ToListAsync(ct);
    }

    public async Task AddAsync(JobMaster job, CancellationToken ct = default)
    {
        await context.JobMasters.AddAsync(job, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(JobMaster job, CancellationToken ct = default)
    {
        context.JobMasters.Update(job);
        await context.SaveChangesAsync(ct);
    }
}
