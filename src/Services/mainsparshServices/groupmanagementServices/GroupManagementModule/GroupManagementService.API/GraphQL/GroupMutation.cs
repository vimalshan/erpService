using MediatR;
using GroupManagementService.Application.Commands;
using GroupManagementService.Application.DTOs;
using HotChocolate;

namespace GroupManagementService.API.GraphQL
{
    /// <summary>
    /// GraphQL Mutation type for group operations
    /// </summary>
    public class GroupMutation
    {
        public async Task<GroupDto> CreateGroup(
            [Service] IMediator mediator,
            string code,
            string name,
            string? description,
            long createdBy,
            bool isAdmin,
            CancellationToken cancellationToken)
        {
            var command = new CreateGroupCommand(code, name, description, createdBy, isAdmin);
            return await mediator.Send(command, cancellationToken);
        }

        public async Task<GroupDto> UpdateGroup(
            [Service] IMediator mediator,
            long groupId,
            string name,
            string? description,
            long updatedBy,
            CancellationToken cancellationToken)
        {
            var command = new UpdateGroupCommand(groupId, name, description, updatedBy);
            return await mediator.Send(command, cancellationToken);
        }

        public async Task<bool> ActivateGroup([Service] IMediator mediator, long groupId, long updatedBy, CancellationToken cancellationToken)
        {
            return await mediator.Send(new ActivateGroupCommand(groupId, updatedBy), cancellationToken);
        }

        public async Task<bool> DeactivateGroup([Service] IMediator mediator, long groupId, long updatedBy, CancellationToken cancellationToken)
        {
            return await mediator.Send(new DeactivateGroupCommand(groupId, updatedBy), cancellationToken);
        }

        public async Task<GroupMenuMapDto> AddMenuMap(
            [Service] IMediator mediator,
            long groupId,
            string menuCode,
            string menuName,
            MenuPermissionsDto permissions,
            long createdBy,
            int? menuSequence,
            CancellationToken cancellationToken)
        {
            var command = new AddMenuMapCommand(groupId, menuCode, menuName, permissions, createdBy, menuSequence);
            return await mediator.Send(command, cancellationToken);
        }

        public async Task<bool> RemoveMenuMap([Service] IMediator mediator, long groupId, string menuCode, long updatedBy, CancellationToken cancellationToken)
        {
            return await mediator.Send(new RemoveMenuMapCommand(groupId, menuCode, updatedBy), cancellationToken);
        }

        public async Task<GroupMenuMapDto> UpdateMenuPermissions(
            [Service] IMediator mediator,
            long groupId,
            string menuCode,
            MenuPermissionsDto permissions,
            long updatedBy,
            CancellationToken cancellationToken)
        {
            var command = new UpdateMenuPermissionsCommand(groupId, menuCode, permissions, updatedBy);
            return await mediator.Send(command, cancellationToken);
        }
    }
}
