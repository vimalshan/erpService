using Microsoft.EntityFrameworkCore;
using RiskService.Domain.Aggregates;
using RiskService.Domain.Interfaces;
using RiskService.Infrastructure.Persistence;

namespace RiskService.Infrastructure.Repositories;

public class RiskRepository(RiskDbContext context) : IRiskRepository
{
    public async Task<RiskAggregate?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        return await context.Risks
            .Include(r => r.Type)
            .Include(r => r.Impact)
            .Include(r => r.Probability)
            .Include(r => r.Rating)
            .Include(r => r.Response)
            .Include(r => r.Causes)
            .Include(r => r.Controls)
            .Include(r => r.ImpactMaps)
            .Include(r => r.Events)
            .Include(r => r.Monitors)
            .Include(r => r.FunctionDetails)
            .Include(r => r.UnitDetails)
            .Include(r => r.Approvals)
            .Include(r => r.Mitigations)
                .ThenInclude(m => m.Actions)
            .FirstOrDefaultAsync(r => r.Id == id, ct);
    }

    public async Task<IReadOnlyList<RiskAggregate>> GetAllAsync(CancellationToken ct = default)
    {
        return await context.Risks
            .Include(r => r.Type)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<IReadOnlyList<RiskAggregate>> GetByOrganizationAsync(long orgId, CancellationToken ct = default)
    {
        return await context.Risks
            .Include(r => r.Type)
            .Where(r => r.OrganizationId == orgId)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task AddAsync(RiskAggregate risk, CancellationToken ct = default)
    {
        await context.Risks.AddAsync(risk, ct);
    }

    public void Update(RiskAggregate risk) => context.Risks.Update(risk);
    public void Delete(RiskAggregate risk) => context.Risks.Remove(risk);
}
