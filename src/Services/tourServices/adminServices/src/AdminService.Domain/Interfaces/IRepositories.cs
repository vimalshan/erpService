using AdminService.Domain.Entities;

namespace AdminService.Domain.Interfaces;

public interface IAdminMasterRepository
{
    Task<AdminMaster?> GetByIdAsync(string adminId, CancellationToken ct = default);
    Task<IReadOnlyList<AdminMaster>> GetAllAsync(CancellationToken ct = default);
    Task<AdminMaster> AddAsync(AdminMaster entity, CancellationToken ct = default);
    Task UpdateAsync(AdminMaster entity, CancellationToken ct = default);
    Task DeleteAsync(string adminId, CancellationToken ct = default);
}

public interface IAdminUserMapRepository
{
    Task<AdminUserMap?> GetByIdAsync(string mapId, CancellationToken ct = default);
    Task<IReadOnlyList<AdminUserMap>> GetByAdminIdAsync(string adminId, CancellationToken ct = default);
    Task<IReadOnlyList<AdminUserMap>> GetAllAsync(CancellationToken ct = default);
    Task<AdminUserMap> AddAsync(AdminUserMap entity, CancellationToken ct = default);
    Task UpdateAsync(AdminUserMap entity, CancellationToken ct = default);
    Task DeleteAsync(string mapId, CancellationToken ct = default);
}

public interface IAdminFinUserMapRepository
{
    Task<AdminFinUserMap?> GetByIdAsync(string financeMapId, CancellationToken ct = default);
    Task<IReadOnlyList<AdminFinUserMap>> GetAllAsync(CancellationToken ct = default);
    Task<AdminFinUserMap> AddAsync(AdminFinUserMap entity, CancellationToken ct = default);
    Task UpdateAsync(AdminFinUserMap entity, CancellationToken ct = default);
    Task DeleteAsync(string financeMapId, CancellationToken ct = default);
}

public interface IAdminAccessRightsRepository
{
    Task<AdminAccessRights?> GetByIdAsync(string rightsId, CancellationToken ct = default);
    Task<IReadOnlyList<AdminAccessRights>> GetByLocationIdAsync(string locationId, CancellationToken ct = default);
    Task<IReadOnlyList<AdminAccessRights>> GetAllAsync(CancellationToken ct = default);
    Task<AdminAccessRights> AddAsync(AdminAccessRights entity, CancellationToken ct = default);
    Task UpdateAsync(AdminAccessRights entity, CancellationToken ct = default);
    Task DeleteAsync(string rightsId, CancellationToken ct = default);
}

public interface IAdminAccessRightsLogRepository
{
    Task<IReadOnlyList<AdminAccessRightsLog>> GetByRightsIdAsync(string rightsId, CancellationToken ct = default);
    Task<AdminAccessRightsLog> AddAsync(AdminAccessRightsLog entity, CancellationToken ct = default);
}
