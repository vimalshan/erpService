using EligibilityService.Domain.Entities;

namespace EligibilityService.Domain.Interfaces;

public interface IShiftMappingRepository
{
    Task<ShiftMapping?> GetAsync(long companyCode, string shiftCode, CancellationToken ct = default);
    Task<IEnumerable<ShiftMapping>> GetAllAsync(long? companyCode = null, CancellationToken ct = default);
    Task AddAsync(ShiftMapping entity, CancellationToken ct = default);
    void Remove(ShiftMapping entity);
}
