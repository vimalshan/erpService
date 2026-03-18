using MasterService.Domain.Entities;
using MasterService.Domain.Interfaces;
using MasterService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MasterService.Infrastructure.Repositories;

public sealed class TrainingRepository(ApplicationDbContext context) : ITrainingRepository
{
    public async Task<TrainingProvider?> GetByCodeAsync(long trainingCode, CancellationToken ct = default)
        => await context.TrainingProviders.FindAsync([trainingCode], ct);

    public async Task<IEnumerable<TrainingProvider>> GetAllActiveAsync(CancellationToken ct = default)
        => await context.TrainingProviders
            .Where(t => t.CancelDate == null)
            .OrderBy(t => t.TrainingName)
            .ToListAsync(ct);

    public async Task AddAsync(TrainingProvider provider, CancellationToken ct = default)
    {
        await context.TrainingProviders.AddAsync(provider, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(TrainingProvider provider, CancellationToken ct = default)
    {
        context.TrainingProviders.Update(provider);
        await context.SaveChangesAsync(ct);
    }
}
