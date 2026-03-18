using Microsoft.EntityFrameworkCore;
using RiskService.Domain.Aggregates;
using RiskService.Domain.Interfaces;
using RiskService.Infrastructure.Persistence;

namespace RiskService.Infrastructure.Repositories;

public class SelfAssessmentRepository(RiskDbContext context) : ISelfAssessmentRepository
{
    public async Task<RiskSelfAssessment?> GetByIdAsync(long id, CancellationToken ct = default)
    {
        return await context.SelfAssessments
            .Include(a => a.EventAssessments)
            .Include(a => a.Comments)
            .FirstOrDefaultAsync(a => a.Id == id, ct);
    }

    public async Task<IReadOnlyList<RiskSelfAssessment>> GetPendingAsync(CancellationToken ct = default)
    {
        return await context.SelfAssessments
            .Where(a => a.Status == 'E' || a.Status == 'P')
            .ToListAsync(ct);
    }

    public async Task AddAsync(RiskSelfAssessment assessment, CancellationToken ct = default)
    {
        await context.SelfAssessments.AddAsync(assessment, ct);
    }

    public void Update(RiskSelfAssessment assessment) => context.SelfAssessments.Update(assessment);
}
