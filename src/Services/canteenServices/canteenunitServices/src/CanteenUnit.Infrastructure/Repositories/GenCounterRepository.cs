using CanteenUnit.Domain.Entities;
using CanteenUnit.Domain.Interfaces;
using CanteenUnit.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CanteenUnit.Infrastructure.Repositories;

public class GenCounterRepository : IGenCounterRepository
{
    private readonly ApplicationDbContext _context;
    public GenCounterRepository(ApplicationDbContext context) => _context = context;

    public async Task<GenCounter?> GetByTypeAsync(string transType, CancellationToken ct)
        => await _context.GenCounters.FirstOrDefaultAsync(g => g.GnTrnTyp == transType, ct);

    public async Task<long> GetNextNumberAsync(string transType, CancellationToken ct)
    {
        var counter = await GetByTypeAsync(transType, ct);
        if (counter is null)
        {
            counter = GenCounter.Create(transType, 1, transType);
            await _context.GenCounters.AddAsync(counter, ct);
        }
        else
        {
            counter.Increment();
            _context.GenCounters.Update(counter);
        }
        await _context.SaveChangesAsync(ct);
        return counter.GnTrnNum ?? 1;
    }
}
