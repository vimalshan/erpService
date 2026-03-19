using Microsoft.EntityFrameworkCore;
using ProductionManagement.Domain.Entities;
using ProductionManagement.Domain.Interfaces;
using ProductionManagement.Infrastructure.Persistence;

namespace ProductionManagement.Infrastructure.Repositories;

public class ProductionPlanRepository : IProductionPlanRepository
{
    private readonly ProductionManagementDbContext _context;

    public ProductionPlanRepository(ProductionManagementDbContext context) => _context = context;

    public async Task<ProductionPlan?> GetByIdAsync(int plantId, int itemId, CancellationToken cancellationToken = default)
    {
        return await _context.ProductionPlans
            .FirstOrDefaultAsync(p => p.ProductionPlantId == plantId && p.SciItemId == itemId, cancellationToken);
    }

    public async Task<IReadOnlyList<ProductionPlan>> GetByPlantIdAsync(int plantId, CancellationToken cancellationToken = default)
    {
        return await _context.ProductionPlans
            .Where(p => p.ProductionPlantId == plantId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProductionPlan>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.ProductionPlans
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<ProductionPlan> AddAsync(ProductionPlan plan, CancellationToken cancellationToken = default)
    {
        await _context.ProductionPlans.AddAsync(plan, cancellationToken);
        return plan;
    }

    public Task UpdateAsync(ProductionPlan plan, CancellationToken cancellationToken = default)
    {
        _context.ProductionPlans.Update(plan);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync(int plantId, int itemId, CancellationToken cancellationToken = default)
    {
        var plan = await _context.ProductionPlans
            .FirstOrDefaultAsync(p => p.ProductionPlantId == plantId && p.SciItemId == itemId, cancellationToken)
            ?? throw new KeyNotFoundException("Production plan not found.");
        _context.ProductionPlans.Remove(plan);
    }
}
