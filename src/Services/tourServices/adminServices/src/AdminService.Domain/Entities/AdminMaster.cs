using AdminService.Domain.Common;

namespace AdminService.Domain.Entities;

public class AdminMaster : BaseEntity
{
    public string AdminId { get; set; } = null!;
    public string AdminName { get; set; } = null!;
    public string AdminPic { get; set; } = null!;
    public string AdminUnitId { get; set; } = null!;
    public string AdminUnitHeadSysId { get; set; } = null!;
    public char? AdminLocStatus { get; set; }

    // Navigation properties
    public ICollection<AdminUserMap> UserMaps { get; set; } = new List<AdminUserMap>();
    public ICollection<AdminAccessRights> AccessRights { get; set; } = new List<AdminAccessRights>();
}
