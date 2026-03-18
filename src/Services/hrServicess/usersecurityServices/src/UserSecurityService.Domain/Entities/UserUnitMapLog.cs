using UserSecurityService.Domain.Common;

namespace UserSecurityService.Domain.Entities;

/// <summary>Audit log snapshot of UserUnitMap changes.</summary>
public class UserUnitMapLog : BaseEntity
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
    public decimal LogCreatedBy { get; private set; }
    public DateTime LogCreatedOn { get; private set; }

    private UserUnitMapLog() { }

    public static UserUnitMapLog FromUnitMap(UserUnitMap map, decimal logCreatedBy)
    {
        return new UserUnitMapLog
        {
            RoleId = map.RoleId,
            RoleApps = map.RoleApps,
            RoleEmpSysId = map.RoleEmpSysId,
            RoleOrgId = map.RoleOrgId,
            RoleUnitAll = map.RoleUnitAll,
            RoleUnitId = map.RoleUnitId,
            RoleMenuGroupId = map.RoleMenuGroupId,
            RoleType = map.RoleType,
            RoleEffDate = map.RoleEffDate,
            RoleClsDate = map.RoleClsDate,
            RoleModifiedBy = map.RoleModifiedBy,
            RoleModifiedOn = map.RoleModifiedOn,
            RoleRemarks = map.RoleRemarks,
            RoleVtcEntry = map.RoleVtcEntry,
            LogCreatedBy = logCreatedBy,
            LogCreatedOn = DateTime.UtcNow
        };
    }
}
