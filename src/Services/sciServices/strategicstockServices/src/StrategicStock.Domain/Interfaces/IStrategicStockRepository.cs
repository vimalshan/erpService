using StrategicStock.Domain.Entities;

namespace StrategicStock.Domain.Interfaces;

public interface IStrategicStockRepository
{
    Task<StrategicStockEntity?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<StrategicStockEntity>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<StrategicStockEntity>> GetByItemAndCompanyAsync(int sciItemId, int companyUnitId, CancellationToken ct = default);
    Task AddAsync(StrategicStockEntity entity, CancellationToken ct = default);
    void Update(StrategicStockEntity entity);
    Task<int> SaveChangesAsync(CancellationToken ct = default);
}
