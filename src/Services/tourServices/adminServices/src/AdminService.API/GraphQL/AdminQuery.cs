using AdminService.Domain.Entities;
using AdminService.Domain.Interfaces;

namespace AdminService.API.GraphQL;

public class AdminQuery
{
    public async Task<IReadOnlyList<AdminMaster>> GetAdminMasters(
        [Service] IAdminMasterRepository repo, CancellationToken ct)
        => await repo.GetAllAsync(ct);

    public async Task<AdminMaster?> GetAdminMasterById(
        string adminId, [Service] IAdminMasterRepository repo, CancellationToken ct)
        => await repo.GetByIdAsync(adminId, ct);

    public async Task<IReadOnlyList<AdminUserMap>> GetAdminUserMaps(
        [Service] IAdminUserMapRepository repo, CancellationToken ct)
        => await repo.GetAllAsync(ct);

    public async Task<IReadOnlyList<AdminUserMap>> GetAdminUserMapsByAdminId(
        string adminId, [Service] IAdminUserMapRepository repo, CancellationToken ct)
        => await repo.GetByAdminIdAsync(adminId, ct);

    public async Task<IReadOnlyList<AdminFinUserMap>> GetAdminFinUserMaps(
        [Service] IAdminFinUserMapRepository repo, CancellationToken ct)
        => await repo.GetAllAsync(ct);

    public async Task<IReadOnlyList<AdminAccessRights>> GetAdminAccessRights(
        [Service] IAdminAccessRightsRepository repo, CancellationToken ct)
        => await repo.GetAllAsync(ct);

    public async Task<AdminAccessRights?> GetAdminAccessRightsById(
        string rightsId, [Service] IAdminAccessRightsRepository repo, CancellationToken ct)
        => await repo.GetByIdAsync(rightsId, ct);

    public async Task<IReadOnlyList<AdminAccessRightsLog>> GetAccessRightsLogs(
        string rightsId, [Service] IAdminAccessRightsLogRepository repo, CancellationToken ct)
        => await repo.GetByRightsIdAsync(rightsId, ct);
}
