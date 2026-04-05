using MediatR;
using GroupManagementService.Application.DTOs;
using GroupManagementService.Application.Queries;
using HotChocolate;

namespace GroupManagementService.API.GraphQL
{
    /// <summary>
    /// GraphQL Query type for group operations
    /// </summary>
    public class GroupQuery
    {
        public async Task<GroupDto?> GetGroupById([Service] IMediator mediator, long groupId, CancellationToken cancellationToken)
        {
            try
            {
                return await mediator.Send(new GetGroupByIdQuery(groupId), cancellationToken);
            }
            catch
            {
                return null;
            }
        }

        public async Task<GroupDto?> GetGroupByCode([Service] IMediator mediator, string groupCode, CancellationToken cancellationToken)
        {
            try
            {
                return await mediator.Send(new GetGroupByCodeQuery(groupCode), cancellationToken);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<GroupDto>> GetAllGroups([Service] IMediator mediator, CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetAllGroupsQuery(), cancellationToken);
        }

        public async Task<IEnumerable<GroupDto>> SearchGroups(
            [Service] IMediator mediator,
            string? searchTerm = null,
            string? status = null,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await mediator.Send(new SearchGroupsQuery(searchTerm, status, pageNumber, pageSize), cancellationToken);
        }

        public async Task<IEnumerable<GroupDto>> GetAdminGroups([Service] IMediator mediator, CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetAdminGroupsQuery(), cancellationToken);
        }

        public async Task<IEnumerable<GroupDto>> GetGroupsByStatus([Service] IMediator mediator, string status, CancellationToken cancellationToken)
        {
            return await mediator.Send(new GetGroupsByStatusQuery(status), cancellationToken);
        }
    }
}
