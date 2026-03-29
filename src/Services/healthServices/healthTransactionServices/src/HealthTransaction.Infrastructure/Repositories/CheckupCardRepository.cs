using HealthTransaction.Domain.Entities;
using HealthTransaction.Domain.Interfaces;
using HealthTransaction.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HealthTransaction.Infrastructure.Repositories;

public class CheckupCardRepository : ICheckupCardRepository
{
    private readonly HealthTransactionDbContext _context;
    public CheckupCardRepository(HealthTransactionDbContext context) => _context = context;

    public async Task<CheckupCard?> GetByHlthNumAsync(decimal hlthNum, CancellationToken cancellationToken = default)
        => await _context.CheckupCards
            .Include(c => c.SubRecords)
            .FirstOrDefaultAsync(c => c.HlthNum == hlthNum, cancellationToken);

    public async Task<IReadOnlyList<CheckupCard>> GetByEmployeeNumAsync(decimal empNum, CancellationToken cancellationToken = default)
        => await _context.CheckupCards.Include(c => c.SubRecords)
            .Where(c => c.EmpNum == empNum).ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<CheckupCard>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.CheckupCards.Include(c => c.SubRecords).ToListAsync(cancellationToken);

    public async Task AddAsync(CheckupCard entity, CancellationToken cancellationToken = default)
        => await _context.CheckupCards.AddAsync(entity, cancellationToken);

    public void Update(CheckupCard entity) => _context.CheckupCards.Update(entity);
    public void Remove(CheckupCard entity) => _context.CheckupCards.Remove(entity);
}
