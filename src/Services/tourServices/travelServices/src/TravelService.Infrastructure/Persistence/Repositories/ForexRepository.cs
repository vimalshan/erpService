using Microsoft.EntityFrameworkCore;
using TravelService.Domain.Entities.Forex;
using TravelService.Domain.Repositories;
using TravelService.Infrastructure.Persistence;

namespace TravelService.Infrastructure.Persistence.Repositories;

public class ForexRepository : IForexRepository
{
    private readonly TravelDbContext _context;

    public ForexRepository(TravelDbContext context) => _context = context;

    public async Task<ForexMain?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        => await _context.ForexMains
            .Include(f => f.Details)
            .Include(f => f.ChequeDetails)
            .FirstOrDefaultAsync(f => f.Id == id, cancellationToken);

    public async Task<IEnumerable<ForexMain>> GetByTourPlanAsync(string tourPlanId, CancellationToken cancellationToken = default)
        => await _context.ForexMains
            .Where(f => f.TourPlanId == tourPlanId)
            .Include(f => f.Details)
            .ToListAsync(cancellationToken);

    public async Task<ForexMain> AddAsync(ForexMain forex, CancellationToken cancellationToken = default)
    {
        await _context.ForexMains.AddAsync(forex, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return forex;
    }

    public async Task UpdateAsync(ForexMain forex, CancellationToken cancellationToken = default)
    {
        _context.ForexMains.Update(forex);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
