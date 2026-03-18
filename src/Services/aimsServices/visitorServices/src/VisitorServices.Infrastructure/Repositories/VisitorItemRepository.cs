using Microsoft.EntityFrameworkCore;
using VisitorServices.Application.Common.Interfaces;
using VisitorServices.Domain.Entities;
using VisitorServices.Infrastructure.Data;

namespace VisitorServices.Infrastructure.Repositories;

public class VisitorItemRepository(VisitorDbContext context) : IVisitorItemRepository
{
    public async Task<long> GetNextIdAsync(CancellationToken cancellationToken = default)
    {
        var max = await context.VisitorItems.MaxAsync(i => (long?)i.Id, cancellationToken) ?? 0L;
        return max + 1;
    }

    public async Task AddAsync(VisitorItem item, CancellationToken cancellationToken = default)
        => await context.VisitorItems.AddAsync(item, cancellationToken);

    public async Task<IEnumerable<VisitorItem>> GetByVisitorIdAsync(long visitorId, CancellationToken cancellationToken = default)
        => await context.VisitorItems
            .Where(i => i.VisitorId == visitorId)
            .ToListAsync(cancellationToken);
}
