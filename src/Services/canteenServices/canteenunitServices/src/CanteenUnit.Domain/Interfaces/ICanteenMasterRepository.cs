using CanteenUnit.Domain.Entities;

namespace CanteenUnit.Domain.Interfaces;

public interface ICanteenMasterRepository
{
    Task<CanteenMaster?> GetByIdAsync(decimal companyCode, CancellationToken ct = default);
    Task<IEnumerable<CanteenMaster>> GetAllAsync(CancellationToken ct = default);
    Task<CanteenMaster> AddAsync(CanteenMaster entity, CancellationToken ct = default);
    void Update(CanteenMaster entity);
    void Delete(CanteenMaster entity);
}
