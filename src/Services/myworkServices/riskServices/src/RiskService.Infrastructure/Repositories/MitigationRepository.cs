using Microsoft.EntityFrameworkCore;
using RiskService.Domain.Aggregates;
using RiskService.Domain.Interfaces;
using RiskService.Infrastructure.Persistence;

namespace RiskService.Infrastructure.Repositories;

public class MitigationRepository(RiskDbContext context) : IMitigationRepository
{
    public async Task<RiskMitigation?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        return await context.Mitigations
            .Include(m => m.Actions)
                .ThenInclude(a => a.Approvals)
            .FirstOrDefaultAsync(m => m.Id == id, ct);
    }

    public async Task<IReadOnlyList<RiskMitigation>> GetByRiskIdAsync(long riskId, CancellationToken ct = default)
    {
        return await context.Mitigations
            .Include(m => m.Actions)
            .Where(m => m.RiskId == riskId)
            .ToListAsync(ct);
    }

    public async Task AddAsync(RiskMitigation mitigation, CancellationToken ct = default)
    {
        await context.Mitigations.AddAsync(mitigation, ct);
    }

    public void Update(RiskMitigation mitigation) => context.Mitigations.Update(mitigation);
}
