using ActionService.Domain.Entities;

namespace ActionService.Domain.Interfaces;

public interface IActionRepository
{
    Task<ActionItem?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<ActionItem>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<ActionItem>> GetByEntityAsync(string entityType, int entityId, CancellationToken ct = default);
    Task<ActionItem> AddAsync(ActionItem entity, CancellationToken ct = default);
    Task UpdateAsync(ActionItem entity, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}
