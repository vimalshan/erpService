using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Domain.Interfaces;

public interface IItemRepository
{
    Task<ItemMaster?> GetByIdAsync(int itemId, CancellationToken ct = default);
    Task<ItemMaster?> GetByOracleCodeAsync(string oracleCode, CancellationToken ct = default);
    Task<IEnumerable<ItemMaster>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<ItemMaster>> GetByProductIdAsync(int productId, CancellationToken ct = default);
    Task<ItemMaster> AddAsync(ItemMaster item, CancellationToken ct = default);
    Task UpdateAsync(ItemMaster item, CancellationToken ct = default);
    Task DeleteAsync(int itemId, CancellationToken ct = default);
    Task<bool> OracleCodeExistsAsync(string oracleCode, CancellationToken ct = default);
}
