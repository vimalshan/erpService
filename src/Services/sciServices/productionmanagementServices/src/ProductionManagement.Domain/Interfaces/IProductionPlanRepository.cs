using ProductionManagement.Domain.Entities;

namespace ProductionManagement.Domain.Interfaces;

public interface IProductionPlanRepository
{
    Task<ProductionPlan?> GetByIdAsync(int plantId, int itemId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProductionPlan>> GetByPlantIdAsync(int plantId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProductionPlan>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ProductionPlan> AddAsync(ProductionPlan plan, CancellationToken cancellationToken = default);
    Task UpdateAsync(ProductionPlan plan, CancellationToken cancellationToken = default);
    Task DeleteAsync(int plantId, int itemId, CancellationToken cancellationToken = default);
}
