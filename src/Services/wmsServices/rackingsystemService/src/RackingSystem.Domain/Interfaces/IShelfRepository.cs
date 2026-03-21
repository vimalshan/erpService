using RackingSystem.Domain.Entities;

namespace RackingSystem.Domain.Interfaces;

public interface IShelfRepository
{
    Task<Shelf?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IEnumerable<Shelf>> GetByRackIdAsync(int rackId, CancellationToken ct = default);
    Task AddAsync(Shelf shelf, CancellationToken ct = default);
    void Update(Shelf shelf);
    void Remove(Shelf shelf);
}
