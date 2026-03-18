namespace GroupManagementService.Application.DTOs
{
    public class GroupDto
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }
        public bool IsAdmin { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public List<GroupMenuMapDto> MenuMaps { get; set; } = new();
    }

    public class GroupMenuMapDto
    {
        public long Id { get; set; }
        public long GroupId { get; set; }
        public string MenuCode { get; set; }
        public string MenuName { get; set; }
        public MenuPermissionsDto Permissions { get; set; }
        public int? MenuSequence { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }

    public class MenuPermissionsDto
    {
        public bool CanView { get; set; }
        public bool CanCreate { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanApprove { get; set; }
    }

    public class CreateGroupRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsAdmin { get; set; }
        public long CreatedBy { get; set; }
    }

    public class UpdateGroupRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public long UpdatedBy { get; set; }
    }

    public class AddMenuMapRequest
    {
        public string MenuCode { get; set; }
        public string MenuName { get; set; }
        public MenuPermissionsDto Permissions { get; set; }
        public int? MenuSequence { get; set; }
        public long CreatedBy { get; set; }
    }

    public class UpdateMenuPermissionsRequest
    {
        public string MenuCode { get; set; }
        public MenuPermissionsDto Permissions { get; set; }
        public long UpdatedBy { get; set; }
    }
}
