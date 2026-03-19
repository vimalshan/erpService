namespace MenuAndSecurityService.Application.DTOs;

public class RoleMenuAccessDto
{
    public long MenuAccessId { get; set; }
    public long MenuId { get; set; }
    public long MenuRoleId { get; set; }
    public long? RoleModifiedBy { get; set; }
    public DateTime? RoleModifiedOn { get; set; }
    public string? MenuName { get; set; }
}
