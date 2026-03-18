using MediatR;
using GroupManagementService.Application.DTOs;

namespace GroupManagementService.Application.Commands
{
    public class CreateGroupCommand : IRequest<GroupDto>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsAdmin { get; set; }
        public long CreatedBy { get; set; }

        public CreateGroupCommand(string code, string name, string? description, long createdBy, bool isAdmin = false)
        {
            Code = code;
            Name = name;
            Description = description;
            CreatedBy = createdBy;
            IsAdmin = isAdmin;
        }
    }

    public class UpdateGroupCommand : IRequest<GroupDto>
    {
        public long GroupId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public long UpdatedBy { get; set; }

        public UpdateGroupCommand(long groupId, string name, string? description, long updatedBy)
        {
            GroupId = groupId;
            Name = name;
            Description = description;
            UpdatedBy = updatedBy;
        }
    }

    public class ActivateGroupCommand : IRequest<bool>
    {
        public long GroupId { get; set; }
        public long UpdatedBy { get; set; }

        public ActivateGroupCommand(long groupId, long updatedBy)
        {
            GroupId = groupId;
            UpdatedBy = updatedBy;
        }
    }

    public class DeactivateGroupCommand : IRequest<bool>
    {
        public long GroupId { get; set; }
        public long UpdatedBy { get; set; }

        public DeactivateGroupCommand(long groupId, long updatedBy)
        {
            GroupId = groupId;
            UpdatedBy = updatedBy;
        }
    }

    public class AddMenuMapCommand : IRequest<GroupMenuMapDto>
    {
        public long GroupId { get; set; }
        public string MenuCode { get; set; }
        public string MenuName { get; set; }
        public MenuPermissionsDto Permissions { get; set; }
        public int? MenuSequence { get; set; }
        public long CreatedBy { get; set; }

        public AddMenuMapCommand(long groupId, string menuCode, string menuName, MenuPermissionsDto permissions, long createdBy, int? menuSequence = null)
        {
            GroupId = groupId;
            MenuCode = menuCode;
            MenuName = menuName;
            Permissions = permissions;
            CreatedBy = createdBy;
            MenuSequence = menuSequence;
        }
    }

    public class RemoveMenuMapCommand : IRequest<bool>
    {
        public long GroupId { get; set; }
        public string MenuCode { get; set; }
        public long UpdatedBy { get; set; }

        public RemoveMenuMapCommand(long groupId, string menuCode, long updatedBy)
        {
            GroupId = groupId;
            MenuCode = menuCode;
            UpdatedBy = updatedBy;
        }
    }

    public class UpdateMenuPermissionsCommand : IRequest<GroupMenuMapDto>
    {
        public long GroupId { get; set; }
        public string MenuCode { get; set; }
        public MenuPermissionsDto Permissions { get; set; }
        public long UpdatedBy { get; set; }

        public UpdateMenuPermissionsCommand(long groupId, string menuCode, MenuPermissionsDto permissions, long updatedBy)
        {
            GroupId = groupId;
            MenuCode = menuCode;
            Permissions = permissions;
            UpdatedBy = updatedBy;
        }
    }
}
