using MediatR;
using GroupManagementService.Application.Commands;
using GroupManagementService.Application.DTOs;

namespace GroupManagementService.API.GraphQL
{
    /// <summary>
    /// GraphQL Mutation type for group operations
    /// </summary>
    public class GroupMutation
    {
        private readonly IMediator _mediator;

        public GroupMutation(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<GroupDto> CreateGroup(
            string code,
            string name,
            string? description,
            long createdBy,
            bool isAdmin,
            CancellationToken cancellationToken)
        {
            var command = new CreateGroupCommand(code, name, description, createdBy, isAdmin);
            return await _mediator.Send(command, cancellationToken);
        }

        public async Task<GroupDto> UpdateGroup(
            long groupId,
            string name,
            string? description,
            long updatedBy,
            CancellationToken cancellationToken)
        {
            var command = new UpdateGroupCommand(groupId, name, description, updatedBy);
            return await _mediator.Send(command, cancellationToken);
        }

        public async Task<bool> ActivateGroup(long groupId, long updatedBy, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new ActivateGroupCommand(groupId, updatedBy), cancellationToken);
        }

        public async Task<bool> DeactivateGroup(long groupId, long updatedBy, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new DeactivateGroupCommand(groupId, updatedBy), cancellationToken);
        }

        public async Task<GroupMenuMapDto> AddMenuMap(
            long groupId,
            string menuCode,
            string menuName,
            MenuPermissionsDto permissions,
            long createdBy,
            int? menuSequence,
            CancellationToken cancellationToken)
        {
            var command = new AddMenuMapCommand(groupId, menuCode, menuName, permissions, createdBy, menuSequence);
            return await _mediator.Send(command, cancellationToken);
        }

        public async Task<bool> RemoveMenuMap(long groupId, string menuCode, long updatedBy, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new RemoveMenuMapCommand(groupId, menuCode, updatedBy), cancellationToken);
        }

        public async Task<GroupMenuMapDto> UpdateMenuPermissions(
            long groupId,
            string menuCode,
            MenuPermissionsDto permissions,
            long updatedBy,
            CancellationToken cancellationToken)
        {
            var command = new UpdateMenuPermissionsCommand(groupId, menuCode, permissions, updatedBy);
            return await _mediator.Send(command, cancellationToken);
        }
    }
}
