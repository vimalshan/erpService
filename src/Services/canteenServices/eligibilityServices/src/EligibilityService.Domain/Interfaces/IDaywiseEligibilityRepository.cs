using EligibilityService.Domain.Entities;

namespace EligibilityService.Domain.Interfaces;

public interface IDaywiseEligibilityRepository
{
    Task<DaywiseEligibility?> GetBySerialNumberAsync(long serialNumber, CancellationToken ct = default);
    Task<IEnumerable<DaywiseEligibility>> GetByEmployeeAsync(long companyCode, long employeeSysId, CancellationToken ct = default);
    Task<IEnumerable<DaywiseEligibility>> GetByDateAsync(long companyCode, DateTime date, CancellationToken ct = default);
    Task AddAsync(DaywiseEligibility entity, CancellationToken ct = default);
    void Update(DaywiseEligibility entity);
    void Remove(DaywiseEligibility entity);
}
