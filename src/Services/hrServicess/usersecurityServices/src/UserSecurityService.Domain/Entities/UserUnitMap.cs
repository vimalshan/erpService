using UserSecurityService.Domain.Common;

namespace UserSecurityService.Domain.Entities;

/// <summary>Maps a role to an organisational unit.</summary>
public class UserUnitMap : BaseEntity
{
    public decimal RoleId { get; private set; }
    public string RoleApps { get; private set; } = null!;
    public decimal RoleEmpSysId { get; private set; }
    public decimal RoleOrgId { get; private set; }
    public char RoleUnitAll { get; private set; }
    public decimal? RoleUnitId { get; private set; }
    public decimal? RoleMenuGroupId { get; private set; }
    public string RoleType { get; private set; } = null!;
    public DateTime RoleEffDate { get; private set; }
    public DateTime? RoleClsDate { get; private set; }
    public decimal RoleModifiedBy { get; private set; }
    public DateTime RoleModifiedOn { get; private set; }
    public string? RoleRemarks { get; private set; }
    public char? RoleVtcEntry { get; private set; }

    private UserUnitMap() { }

    public static UserUnitMap Create(
        decimal roleId, string apps, decimal empSysId, decimal orgId,
        char unitAll, string roleType, DateTime effDate, decimal createdBy,
        decimal? unitId = null, decimal? menuGroupId = null, string? remarks = null, char? vtcEntry = null)
    {
        return new UserUnitMap
        {
            RoleId = roleId,
            RoleApps = apps,
            RoleEmpSysId = empSysId,
            RoleOrgId = orgId,
            RoleUnitAll = unitAll,
            RoleUnitId = unitId,
            RoleMenuGroupId = menuGroupId,
            RoleType = roleType,
            RoleEffDate = effDate,
            RoleModifiedBy = createdBy,
            RoleModifiedOn = DateTime.UtcNow,
            RoleRemarks = remarks,
            RoleVtcEntry = vtcEntry
        };
    }

    public void Close(decimal modifiedBy)
    {
        RoleClsDate = DateTime.UtcNow;
        RoleModifiedBy = modifiedBy;
        RoleModifiedOn = DateTime.UtcNow;
    }
}
