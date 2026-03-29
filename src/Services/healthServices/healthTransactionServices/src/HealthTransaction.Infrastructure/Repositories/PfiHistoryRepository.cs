using HealthTransaction.Domain.Entities;
using HealthTransaction.Domain.Interfaces;
using HealthTransaction.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HealthTransaction.Infrastructure.Repositories;

public class PfiHistoryRepository : IPfiHistoryRepository
{
    private readonly HealthTransactionDbContext _context;
    public PfiHistoryRepository(HealthTransactionDbContext context) => _context = context;

    public async Task<IReadOnlyList<PfiHistory>> GetByHlthNumAsync(decimal hlthNum, CancellationToken cancellationToken = default)
        => await _context.PfiHistories.Where(p => p.HlthNum == hlthNum).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<PfiHistory>> GetByEmployeeNumAsync(decimal empNum, CancellationToken cancellationToken = default)
        => await _context.PfiHistories.Where(p => p.EmpNum == empNum).ToListAsync(cancellationToken);

    public async Task AddAsync(PfiHistory entity, CancellationToken cancellationToken = default)
        => await _context.PfiHistories.AddAsync(entity, cancellationToken);

    public void Update(PfiHistory entity) => _context.PfiHistories.Update(entity);
}
