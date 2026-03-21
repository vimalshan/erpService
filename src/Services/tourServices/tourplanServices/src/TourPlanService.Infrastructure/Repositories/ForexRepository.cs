using Microsoft.EntityFrameworkCore;
using TourPlanService.Domain.Entities;
using TourPlanService.Domain.Interfaces;
using TourPlanService.Infrastructure.Data;

namespace TourPlanService.Infrastructure.Repositories;

public sealed class ForexRepository(TourPlanDbContext context) : IForexRepository
{
    public async Task<ForexRequisition?> GetByIdAsync(string forReqId, CancellationToken cancellationToken = default) =>
        await context.ForexRequisitions
            .Include(x => x.Details)
            .Include(x => x.ChequeDetails)
            .FirstOrDefaultAsync(x => x.ForReqId == forReqId, cancellationToken);

    public async Task<IEnumerable<ForexRequisition>> GetByTourPlanIdAsync(string tpId, CancellationToken cancellationToken = default) =>
        await context.ForexRequisitions
            .Where(x => x.ForReqTpId == tpId)
            .Include(x => x.Details)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(ForexRequisition forex, CancellationToken cancellationToken = default) =>
        await context.ForexRequisitions.AddAsync(forex, cancellationToken);

    public void Update(ForexRequisition forex) => context.ForexRequisitions.Update(forex);
}
