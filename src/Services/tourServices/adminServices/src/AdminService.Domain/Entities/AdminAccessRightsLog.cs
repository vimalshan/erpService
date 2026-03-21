using AdminService.Domain.Common;

namespace AdminService.Domain.Entities;

public class AdminAccessRightsLog : BaseEntity
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

    // Navigation property
    public AdminAccessRights AccessRights { get; set; } = null!;
}
