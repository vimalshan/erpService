using Microsoft.EntityFrameworkCore;
using PFTransactionalService.Domain.Entities;
using PFTransactionalService.Domain.Interfaces;
using PFTransactionalService.Infrastructure.Persistence.EfCore;

namespace PFTransactionalService.Infrastructure.Repositories;

public class PFSettlementRepository : IPFSettlementRepository
{
    private readonly PFTransactionalDbContext _context;

    public PFSettlementRepository(PFTransactionalDbContext context)
    {
        _context = context;
    }

    public async Task<PFSettlement?> GetByIdAsync(long settlementId, CancellationToken cancellationToken = default)
    {
        return await _context.PFSettlements
            .Include(s => s.Transactions)
            .FirstOrDefaultAsync(s => s.PfSettlementId == settlementId, cancellationToken);
    }

    public async Task<IEnumerable<PFSettlement>> GetByEmpSysIdAsync(long empSysId, CancellationToken cancellationToken = default)
    {
        return await _context.PFSettlements
            .Include(s => s.Transactions)
            .Where(s => s.EmpSysId == empSysId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<PFSettlement>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.PFSettlements
            .Include(s => s.Transactions)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(PFSettlement settlement, CancellationToken cancellationToken = default)
    {
        await _context.PFSettlements.AddAsync(settlement, cancellationToken);
    }

    public Task UpdateAsync(PFSettlement settlement, CancellationToken cancellationToken = default)
    {
        _context.PFSettlements.Update(settlement);
        return Task.CompletedTask;
    }
}
