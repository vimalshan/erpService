using ExpenseService.Domain.Entities;
using ExpenseService.Domain.Interfaces;
using ExpenseService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseService.Infrastructure.Repositories;

public class DaRuleRepository : IDaRuleRepository
{
    private readonly ExpenseDbContext _context;

    public DaRuleRepository(ExpenseDbContext context)
    {
        _context = context;
    }

    public async Task<DaRule?> GetByIdAsync(long serialNumber, CancellationToken ct = default)
    {
        return await _context.DaRules.FirstOrDefaultAsync(r => r.SerialNumber == serialNumber, ct);
    }

    public async Task<IReadOnlyList<DaRule>> GetActiveRulesAsync(long bandId, CancellationToken ct = default)
    {
        return await _context.DaRules
            .Where(r => r.BandId == bandId && (r.ClosureDate == null || r.ClosureDate >= DateTime.UtcNow))
            .ToListAsync(ct);
    }

    public async Task<DaRule> AddAsync(DaRule rule, CancellationToken ct = default)
    {
        _context.DaRules.Add(rule);
        await _context.SaveChangesAsync(ct);
        return rule;
    }

    public async Task UpdateAsync(DaRule rule, CancellationToken ct = default)
    {
        _context.DaRules.Update(rule);
        await _context.SaveChangesAsync(ct);
    }
}
