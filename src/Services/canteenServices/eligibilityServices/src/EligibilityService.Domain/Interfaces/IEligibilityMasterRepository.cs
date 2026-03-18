using EligibilityService.Domain.Entities;

namespace EligibilityService.Domain.Interfaces;

public interface IEligibilityMasterRepository
{
    Task<EligibilityMaster?> GetAsync(long canteenUnit, string shiftCode, decimal itemCode, CancellationToken ct = default);
    Task<IEnumerable<EligibilityMaster>> GetAllAsync(long? canteenUnit = null, CancellationToken ct = default);
    Task AddAsync(EligibilityMaster entity, CancellationToken ct = default);
    void Update(EligibilityMaster entity);
    void Remove(EligibilityMaster entity);
    Task<bool> ExistsAsync(long canteenUnit, string shiftCode, decimal itemCode, CancellationToken ct = default);
}
