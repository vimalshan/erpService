using AdminService.Domain.Common;

namespace AdminService.Domain.Entities;

public class AdminUserMap : BaseEntity
{
    public string AdminMapId { get; set; } = null!;
    public string AdminBookType { get; set; } = null!;
    public string AdminMode { get; set; } = null!;
    public string AdminEmpSysId { get; set; } = null!;
    public string AdminId { get; set; } = null!;
    public string AdminLastModifiedBy { get; set; } = null!;
    public DateTime AdminLastModifiedOn { get; set; }

    // Navigation property
    public AdminMaster Admin { get; set; } = null!;
}
