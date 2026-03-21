namespace AdminService.Application.DTOs;

public class AdminMasterDto
{
    public string AdminId { get; set; } = null!;
    public string AdminName { get; set; } = null!;
    public string AdminPic { get; set; } = null!;
    public string AdminUnitId { get; set; } = null!;
    public string AdminUnitHeadSysId { get; set; } = null!;
    public string? AdminLocStatus { get; set; }
}

public class AdminUserMapDto
{
    public string AdminMapId { get; set; } = null!;
    public string AdminBookType { get; set; } = null!;
    public string AdminMode { get; set; } = null!;
    public string AdminEmpSysId { get; set; } = null!;
    public string AdminId { get; set; } = null!;
    public string AdminLastModifiedBy { get; set; } = null!;
    public DateTime AdminLastModifiedOn { get; set; }
}

public class AdminFinUserMapDto
{
    public string FinanceMapId { get; set; } = null!;
    public string FinancePayUnitId { get; set; } = null!;
    public string FinanceEmpSysId { get; set; } = null!;
    public string? FinanceLastModifiedBy { get; set; }
    public DateTime? FinanceLastModifiedOn { get; set; }
}

public class AdminAccessRightsDto
{
    public string AdminRightsId { get; set; } = null!;
    public string? AdminLocationId { get; set; }
    public string? AdminRightsFor { get; set; }
    public string? AdminRightsType { get; set; }
    public string? AdminUserId { get; set; }
    public string? AdminAlertId { get; set; }
    public string? AdminContactNo { get; set; }
    public string? AdminContactDes { get; set; }
    public DateTime? AdminEntOn { get; set; }
    public string? AdminEntBy { get; set; }
}

public class AdminAccessRightsLogDto
{
    public string AdminLogId { get; set; } = null!;
    public string AdminRightsId { get; set; } = null!;
    public string? AdminLocationId { get; set; }
    public string? AdminRightsFor { get; set; }
    public string? AdminRightsType { get; set; }
    public string? AdminUserId { get; set; }
    public string? AdminAlertId { get; set; }
    public string? AdminContactNo { get; set; }
    public string? AdminContactDes { get; set; }
    public DateTime? AdminEntOn { get; set; }
    public string? AdminEntBy { get; set; }
}
