using EligibilityService.Domain.Entities;

namespace EligibilityService.Domain.Interfaces;

public interface IEligibilityMasterHistoryRepository
{
    Task AddAsync(EligibilityMasterHistory entity, CancellationToken ct = default);
    Task<IEnumerable<EligibilityMasterHistory>> GetHistoryAsync(long canteenUnit, string shiftCode, decimal itemCode, CancellationToken ct = default);
}
