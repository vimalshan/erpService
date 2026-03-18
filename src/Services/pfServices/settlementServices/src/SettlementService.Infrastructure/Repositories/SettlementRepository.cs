using Microsoft.EntityFrameworkCore;
using SettlementService.Domain.Aggregates;
using SettlementService.Domain.Interfaces;
using SettlementService.Infrastructure.Persistence.EfCore;

namespace SettlementService.Infrastructure.Repositories;

public class SettlementRepository : ISettlementRepository
{
    private readonly SettlementDbContext _context;

    public SettlementRepository(SettlementDbContext context)
    {
        _context = context;
    }

    public async Task<Settlement?> GetByIdAsync(long settlementNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Settlements
            .Include(s => s.Deductions)
            .Include(s => s.Approvals)
            .Include(s => s.Payments)
            .FirstOrDefaultAsync(s => s.StSetNum == settlementNumber, cancellationToken);
    }

    public async Task<IEnumerable<Settlement>> GetByMemberNoAsync(long memberNo, CancellationToken cancellationToken = default)
    {
        return await _context.Settlements
            .Include(s => s.Deductions)
            .Include(s => s.Approvals)
            .Include(s => s.Payments)
            .Where(s => s.StMemberNo == memberNo)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Settlement>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Settlements
            .Include(s => s.Deductions)
            .Include(s => s.Approvals)
            .Include(s => s.Payments)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Settlement settlement, CancellationToken cancellationToken = default)
    {
        await _context.Settlements.AddAsync(settlement, cancellationToken);
    }

    public Task UpdateAsync(Settlement settlement, CancellationToken cancellationToken = default)
    {
        _context.Settlements.Update(settlement);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(long settlementNumber, CancellationToken cancellationToken = default)
    {
        var settlement = await GetByIdAsync(settlementNumber, cancellationToken);
        if (settlement != null)
            _context.Settlements.Remove(settlement);
    }
}
