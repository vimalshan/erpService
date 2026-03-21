using UnitService.Domain.Entities;

namespace UnitService.Domain.Interfaces;

public interface IAccessRepository
{
    Task<AccessMaster?> GetByIdAsync(int accessId, CancellationToken ct = default);
    Task<IEnumerable<AccessMaster>> GetByUnitCodeAsync(string unitCode, CancellationToken ct = default);
    Task<IEnumerable<AccessMaster>> GetByEmployeeAsync(int employeeSysId, CancellationToken ct = default);
    Task<int> GetNextIdAsync(CancellationToken ct = default);
    Task AddAsync(AccessMaster access, CancellationToken ct = default);
    void Update(AccessMaster access);
}
