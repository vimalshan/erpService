using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Domain.Interfaces;

public interface IProductRepository
{
    Task<MainProductMaster?> GetByIdAsync(int productId, CancellationToken ct = default);
    Task<IEnumerable<MainProductMaster>> GetAllAsync(CancellationToken ct = default);
    Task<MainProductMaster> AddAsync(MainProductMaster product, CancellationToken ct = default);
    Task UpdateAsync(MainProductMaster product, CancellationToken ct = default);
    Task DeleteAsync(int productId, CancellationToken ct = default);
    Task<bool> ExistsAsync(int productId, CancellationToken ct = default);
}
