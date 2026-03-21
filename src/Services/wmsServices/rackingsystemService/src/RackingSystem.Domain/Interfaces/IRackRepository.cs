using RackingSystem.Domain.Entities;

namespace RackingSystem.Domain.Interfaces;

public interface IRackRepository
{
    Task<Rack?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<Rack>> GetAllAsync(CancellationToken ct = default);
    Task<IEnumerable<Rack>> GetByZoneIdAsync(int zoneId, CancellationToken ct = default);
    Task<bool> ExistsAsync(int zoneId, string code, CancellationToken ct = default);
    Task AddAsync(Rack rack, CancellationToken ct = default);
    void Update(Rack rack);
    void Remove(Rack rack);
}
