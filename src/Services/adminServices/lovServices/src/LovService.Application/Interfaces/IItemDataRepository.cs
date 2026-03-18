using LovService.Domain.Entities;

namespace LovService.Application.Interfaces;

public interface IItemDataRepository
{
    Task<ItemData?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<ItemData>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<ItemData>> SearchAsync(string? catName, string? itemName, CancellationToken ct = default);
    Task AddAsync(ItemData itemData, CancellationToken ct = default);
    void Update(ItemData itemData);
    void Delete(ItemData itemData);
}
