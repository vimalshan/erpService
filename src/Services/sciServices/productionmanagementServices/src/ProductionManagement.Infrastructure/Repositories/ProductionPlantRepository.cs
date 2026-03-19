using Microsoft.EntityFrameworkCore;
using ProductionManagement.Domain.Entities;
using ProductionManagement.Domain.Interfaces;
using ProductionManagement.Infrastructure.Persistence;

namespace ProductionManagement.Infrastructure.Repositories;

public class ProductionPlantRepository : IProductionPlantRepository
{
    private readonly ProductionManagementDbContext _context;

    public ProductionPlantRepository(ProductionManagementDbContext context) => _context = context;

    public async Task<ProductionPlant?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.ProductionPlants
            .Include(p => p.ProductionPlans)
            .Include(p => p.ProductMaps)
            .FirstOrDefaultAsync(p => p.ProductionPlantId == id, cancellationToken);
    }

    public async Task<IReadOnlyList<ProductionPlant>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ProductionPlants
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductionPlant> AddAsync(ProductionPlant plant, CancellationToken cancellationToken = default)
    {
        await _context.ProductionPlants.AddAsync(plant, cancellationToken);
        return plant;
    }

    public Task UpdateAsync(ProductionPlant plant, CancellationToken cancellationToken = default)
    {
        _context.ProductionPlants.Update(plant);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var plant = await _context.ProductionPlants.FindAsync(new object[] { id }, cancellationToken)
            ?? throw new KeyNotFoundException($"Production plant {id} not found.");
        _context.ProductionPlants.Remove(plant);
    }
}
