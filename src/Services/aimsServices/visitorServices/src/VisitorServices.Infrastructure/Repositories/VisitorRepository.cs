using Microsoft.EntityFrameworkCore;
using VisitorServices.Application.Common.Interfaces;
using VisitorServices.Domain.Aggregates;
using VisitorServices.Domain.Enums;
using VisitorServices.Infrastructure.Data;

namespace VisitorServices.Infrastructure.Repositories;

public class VisitorRepository(VisitorDbContext context) : IVisitorRepository
{
    public async Task<VisitorAggregate?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        => await context.Visitors
            .FirstOrDefaultAsync(v => v.Id == id, cancellationToken);

    public async Task<IEnumerable<VisitorAggregate>> GetActiveVisitorsAsync(CancellationToken cancellationToken = default)
        => await context.Visitors
            .Where(v => v.Status == VisitorStatus.Inside)
            .OrderByDescending(v => v.CheckInTime)
            .ToListAsync(cancellationToken);

    public async Task<long> GetNextIdAsync(CancellationToken cancellationToken = default)
    {
        var max = await context.Visitors.MaxAsync(v => (long?)v.Id, cancellationToken) ?? 0L;
        return max + 1;
    }

    public async Task AddAsync(VisitorAggregate visitor, CancellationToken cancellationToken = default)
        => await context.Visitors.AddAsync(visitor, cancellationToken);

    public void Update(VisitorAggregate visitor)
        => context.Visitors.Update(visitor);
}
