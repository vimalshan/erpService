using FillingOperationService.Domain.Entities;
using FillingOperationService.Domain.Interfaces;
using FillingOperationService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FillingOperationService.Infrastructure.Repositories;

public class FillingLineRepository(FillingOperationsDbContext context) : IFillingLineRepository
{
    public async Task<FillingLine?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await context.FillingLines.FirstOrDefaultAsync(l => l.FillingLineId == id, cancellationToken);

    public async Task<IEnumerable<FillingLine>> GetByPlantIdAsync(int plantId, CancellationToken cancellationToken = default)
        => await context.FillingLines
            .AsNoTracking()
            .Where(l => l.FillingPlantId == plantId)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(FillingLine line, CancellationToken cancellationToken = default)
        => await context.FillingLines.AddAsync(line, cancellationToken);

    public void Update(FillingLine line)
        => context.FillingLines.Update(line);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await context.SaveChangesAsync(cancellationToken);
}
