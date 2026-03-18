using Microsoft.EntityFrameworkCore;
using ScholarshipService.Domain.Entities;
using ScholarshipService.Domain.Repositories;
using ScholarshipService.Infrastructure.Data;

namespace ScholarshipService.Infrastructure.Repositories;

public class ScholarshipDetailRepository(ScholarshipDbContext context) : IScholarshipDetailRepository
{
    public async Task<ScholarshipDetail?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => await context.ScholarshipDetails.FindAsync([id], cancellationToken);

    public async Task<IEnumerable<ScholarshipDetail>> GetByMainIdAsync(int mainId, CancellationToken cancellationToken = default)
        => await context.ScholarshipDetails
            .Where(x => x.MainId == mainId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<long> GetNextIdAsync(CancellationToken cancellationToken = default)
    {
        var maxId = await context.ScholarshipDetails.MaxAsync(x => (long?)x.Id, cancellationToken);
        return (maxId ?? 0L) + 1L;
    }

    public async Task AddAsync(ScholarshipDetail detail, CancellationToken cancellationToken = default)
        => await context.ScholarshipDetails.AddAsync(detail, cancellationToken);

    public Task UpdateAsync(ScholarshipDetail detail, CancellationToken cancellationToken = default)
    {
        context.ScholarshipDetails.Update(detail);
        return Task.CompletedTask;
    }
}
