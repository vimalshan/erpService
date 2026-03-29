using HealthTransaction.Domain.Entities;

namespace HealthTransaction.Domain.Interfaces;

public interface IDynamicHealthDetailRepository
{
    Task<IReadOnlyList<DynamicHealthDetail>> GetByHlthNumAsync(decimal hlthNum, CancellationToken cancellationToken = default);
    Task<DynamicHealthDetail?> GetByKeyAsync(decimal hlthNum, string chkupCod, string comCode, decimal ctrlSrcId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DynamicHealthDetail>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(DynamicHealthDetail entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<DynamicHealthDetail> entities, CancellationToken cancellationToken = default);
    void Update(DynamicHealthDetail entity);
    void Remove(DynamicHealthDetail entity);
}
