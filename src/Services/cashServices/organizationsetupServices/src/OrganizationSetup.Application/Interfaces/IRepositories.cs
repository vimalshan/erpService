using OrganizationSetup.Domain.Entities;

namespace OrganizationSetup.Application.Interfaces;

public interface IRoleRepository
{
    Task<DealRole?> GetByIdAsync(long roleId, CancellationToken ct = default);
    Task<IEnumerable<DealRole>> GetAllAsync(CancellationToken ct = default);
    Task<DealRole?> GetByNameAsync(string roleName, CancellationToken ct = default);
    Task AddAsync(DealRole role, CancellationToken ct = default);
    Task UpdateAsync(DealRole role, CancellationToken ct = default);
    Task DeleteAsync(long roleId, CancellationToken ct = default);
}

public interface IUserMapRepository
{
    Task<DealUserMap?> GetByIdAsync(long mapId, CancellationToken ct = default);
    Task<IEnumerable<DealUserMap>> GetByOrgAsync(long orgId, CancellationToken ct = default);
    Task<IEnumerable<DealUserMap>> GetByEmployeeAsync(long empSysId, CancellationToken ct = default);
    Task AddAsync(DealUserMap map, CancellationToken ct = default);
    Task UpdateAsync(DealUserMap map, CancellationToken ct = default);
    Task DeleteAsync(long mapId, CancellationToken ct = default);
}

public interface IOrgParamsRepository
{
    Task<DealOrgParams?> GetByIdAsync(long paramId, CancellationToken ct = default);
    Task<IEnumerable<DealOrgParams>> GetByOrgAsync(long orgId, CancellationToken ct = default);
    Task<DealOrgParams?> GetByTypeAsync(long orgId, string paramType, CancellationToken ct = default);
    Task AddAsync(DealOrgParams param, CancellationToken ct = default);
    Task UpdateAsync(DealOrgParams param, CancellationToken ct = default);
    Task DeleteAsync(long paramId, CancellationToken ct = default);
}

public interface IPpLimitRepository
{
    Task<DealPpLimit?> GetByIdAsync(long limitId, CancellationToken ct = default);
    Task<IEnumerable<DealPpLimit>> GetByOrgAndYearAsync(long orgId, int finYear, CancellationToken ct = default);
    Task AddAsync(DealPpLimit limit, CancellationToken ct = default);
    Task UpdateAsync(DealPpLimit limit, CancellationToken ct = default);
    Task DeleteAsync(long limitId, CancellationToken ct = default);
}
