using ProductionManagement.Domain.Entities;

namespace ProductionManagement.Domain.Interfaces;

public interface IProductionPlantRepository
{
    Task<ProductionPlant?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ProductionPlant>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ProductionPlant> AddAsync(ProductionPlant plant, CancellationToken cancellationToken = default);
    Task UpdateAsync(ProductionPlant plant, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
