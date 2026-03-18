using CardManagement.Domain.Entities;

namespace CardManagement.Domain.Interfaces;

public interface ICanteenCardMapRepository
{
    Task<CanteenCardMap?> GetByIdAsync(decimal sysId, CancellationToken ct = default);
    Task<IEnumerable<CanteenCardMap>> GetByCanteenUnitAsync(long canteenUnit, CancellationToken ct = default);
    Task<IEnumerable<CanteenCardMap>> GetActiveByCanteenUnitAsync(long canteenUnit, CancellationToken ct = default);
    Task AddAsync(CanteenCardMap entity, CancellationToken ct = default);
    void Update(CanteenCardMap entity);
}
