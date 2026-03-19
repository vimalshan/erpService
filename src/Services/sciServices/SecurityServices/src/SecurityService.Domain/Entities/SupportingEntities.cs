namespace SecurityService.Domain.Entities;

/// <summary>Maps to ACCESS_ROLE table.</summary>
public sealed class AccessRole
{
    public string? UserCode { get; set; }    // RA_USR_COD
    public long? UserId { get; set; }        // RA_USR_NUM
    public long? RoleId { get; set; }        // RA_ROL_COD
    public string? UpdatedByCode { get; set; }
    public long? UpdatedByNum { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

/// <summary>Maps to ACCESS_ROLE_MASTER table.</summary>
public sealed class AccessRoleMaster
{
    public long? RoleId { get; set; }        // AR_ROL_COD
    public string? RoleName { get; set; }    // AR_ROL_NAM
    public string? UpdatedByCode { get; set; }
    public long? UpdatedByNum { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>Maps to ACCESSROLE_MENU table.</summary>
public sealed class AccessRoleMenu
{
    public long? RoleId { get; set; }   // ARM_ROL_COD
    public long? MenuId { get; set; }   // ARM_MEN_COD
    public string? UpdatedByCode { get; set; }
    public long? UpdatedByNum { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

/// <summary>Maps to MENUMASTER table.</summary>
public sealed class MenuMaster
{
    public long? MenuId { get; set; }          // MENU_ID
    public string? MenuName { get; set; }      // MENU_NAME
    public string? Url { get; set; }           // URL
    public long? ParentMenuId { get; set; }    // PARENT_MENU_ID
    public long? DisplayOrder { get; set; }    // DISPLAYORDER
}

/// <summary>Maps to USER_MASTER_MAP table.</summary>
public sealed class UserMasterMap
{
    public long MapId { get; set; }           // UM_MAP_ID
    public long UserId { get; set; }          // UM_USR_NUM
    public string DepartmentCode { get; set; } = null!; // UM_DEPT_COD
    public DateTime StartDate { get; set; }   // UM_STR_DAT
    public DateTime? EndDate { get; set; }    // UM_END_DAT
}
