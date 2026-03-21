using Microsoft.EntityFrameworkCore;
using TravelRequestService.Domain.Entities;
using TravelRequestService.Domain.Interfaces;
using TravelRequestService.Infrastructure.Data;

namespace TravelRequestService.Infrastructure.Repositories;

public class TravelAdvanceRepository : ITravelAdvanceRepository
{
    private readonly TravelDbContext _context;

    public TravelAdvanceRepository(TravelDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<TravelAdvance>> GetByRequestAsync(long requestNumber, CancellationToken cancellationToken = default)
    {
        return await _context.TravelAdvances
            .Where(a => a.RequestNumber == requestNumber)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(TravelAdvance advance, CancellationToken cancellationToken = default)
    {
        await _context.TravelAdvances.AddAsync(advance, cancellationToken);
    }

    public Task UpdateAsync(TravelAdvance advance, CancellationToken cancellationToken = default)
    {
        _context.TravelAdvances.Update(advance);
        return Task.CompletedTask;
    }
}
