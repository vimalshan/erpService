using CanteenUnit.Domain.Entities;

namespace CanteenUnit.Domain.Interfaces;

public interface ICanteenUnitRepository
{
    Task<CanteenUnitMaster?> GetByIdAsync(decimal companyCode, CancellationToken ct = default);
    Task<IEnumerable<CanteenUnitMaster>> GetAllAsync(CancellationToken ct = default);
    Task<CanteenUnitMaster> AddAsync(CanteenUnitMaster entity, CancellationToken ct = default);
    void Update(CanteenUnitMaster entity);
    void Delete(CanteenUnitMaster entity);
    Task<bool> ExistsAsync(decimal companyCode, CancellationToken ct = default);
}
