namespace OrganizationSetup.Application.DTOs;

public class RoleDto
{
    public long RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public long RoleLevel { get; set; }
    public decimal RoleModifiedBy { get; set; }
    public DateTime RoleModifiedOn { get; set; }
}
