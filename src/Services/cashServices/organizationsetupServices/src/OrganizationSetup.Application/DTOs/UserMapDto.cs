namespace OrganizationSetup.Application.DTOs;

public class UserMapDto
{
    public long RoleMapId { get; set; }
    public long RoleId { get; set; }
    public long RoleEmpSysId { get; set; }
    public long RoleOrgId { get; set; }
    public long? RoleBusiness { get; set; }
    public string RoleName { get; set; } = string.Empty;
}
