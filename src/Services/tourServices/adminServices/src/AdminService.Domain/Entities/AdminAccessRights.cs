using AdminService.Domain.Common;

namespace AdminService.Domain.Entities;

public class AdminAccessRights : BaseEntity
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

    // Navigation properties
    public AdminMaster? Admin { get; set; }
    public ICollection<AdminAccessRightsLog> AccessRightsLogs { get; set; } = new List<AdminAccessRightsLog>();
}
