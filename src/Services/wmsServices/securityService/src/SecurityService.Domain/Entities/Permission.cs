using SecurityService.Domain.Common;

namespace SecurityService.Domain.Entities;

public class Permission : BaseEntity
{
    public int PermissionId { get; set; }
    public string PermissionName { get; set; } = string.Empty;
    public string? Module { get; set; }
    public string? Description { get; set; }

    // Navigation properties
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
