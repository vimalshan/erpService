namespace MenuAndSecurityService.Application.DTOs;

public class MenuMasterDto
{
    public long MenuId { get; set; }
    public string MenuName { get; set; } = string.Empty;
    public string MenuPageName { get; set; } = string.Empty;
    public long? MenuParentId { get; set; }
    public int MenuDisplayOrder { get; set; }
    public long ModifiedBy { get; set; }
    public DateTime ModifiedOn { get; set; }
    public List<MenuMasterDto> Children { get; set; } = new();
}
