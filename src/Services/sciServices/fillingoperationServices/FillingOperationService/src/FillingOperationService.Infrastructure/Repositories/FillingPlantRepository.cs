using FillingOperationService.Domain.Entities;
using FillingOperationService.Domain.Interfaces;
using FillingOperationService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FillingOperationService.Infrastructure.Repositories;

public class FillingPlantRepository(FillingOperationsDbContext context) : IFillingPlantRepository
{
    public async Task<FillingPlant?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        => await context.FillingPlants
            .Include(p => p.FillingLines)
            .FirstOrDefaultAsync(p => p.FillingPlantId == id, cancellationToken);

    public async Task<IEnumerable<FillingPlant>> GetAllAsync(CancellationToken cancellationToken = default)
        => await context.FillingPlants.AsNoTracking().ToListAsync(cancellationToken);

    public async Task<IEnumerable<FillingPlant>> GetByCompanyUnitAsync(int companyUnitId, CancellationToken cancellationToken = default)
        => await context.FillingPlants
            .AsNoTracking()
            .Where(p => p.CompanyUnitId == companyUnitId)
            .ToListAsync(cancellationToken);

    public async Task AddAsync(FillingPlant plant, CancellationToken cancellationToken = default)
        => await context.FillingPlants.AddAsync(plant, cancellationToken);

    public void Update(FillingPlant plant)
        => context.FillingPlants.Update(plant);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await context.SaveChangesAsync(cancellationToken);
}
