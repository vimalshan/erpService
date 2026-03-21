using UnitService.Domain.Entities;

namespace UnitService.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<CategoryMaster?> GetByUnitCodeAsync(string unitCode, CancellationToken ct = default);
    Task<IEnumerable<CategoryMaster>> GetAllAsync(CancellationToken ct = default);
    Task AddAsync(CategoryMaster category, CancellationToken ct = default);
    void Update(CategoryMaster category);
}
