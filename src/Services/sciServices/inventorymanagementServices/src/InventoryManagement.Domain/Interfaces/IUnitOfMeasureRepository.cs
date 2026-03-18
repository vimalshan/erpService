using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Domain.Interfaces;

public interface IUnitOfMeasureRepository
{
    Task<UnitOfMeasure?> GetByIdAsync(int unitId, CancellationToken ct = default);
    Task<IEnumerable<UnitOfMeasure>> GetAllAsync(CancellationToken ct = default);
}
