using MediatR;
using GroupManagementService.Application.DTOs;
using GroupManagementService.Application.Queries;

namespace GroupManagementService.API.GraphQL
{
    /// <summary>
    /// GraphQL Query type for group operations
    /// </summary>
    public class GroupQuery
    {
        private readonly IMediator _mediator;

        public GroupQuery(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<GroupDto?> GetGroupById(long groupId, CancellationToken cancellationToken)
        {
            try
            {
                return await _mediator.Send(new GetGroupByIdQuery(groupId), cancellationToken);
            }
            catch
            {
                return null;
            }
        }

        public async Task<GroupDto?> GetGroupByCode(string groupCode, CancellationToken cancellationToken)
        {
            try
            {
                return await _mediator.Send(new GetGroupByCodeQuery(groupCode), cancellationToken);
            }
            catch
            {
                return null;
            }
        }

        public async Task<IEnumerable<GroupDto>> GetAllGroups(CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetAllGroupsQuery(), cancellationToken);
        }

        public async Task<IEnumerable<GroupDto>> SearchGroups(
            string? searchTerm = null,
            string? status = null,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            return await _mediator.Send(new SearchGroupsQuery(searchTerm, status, pageNumber, pageSize), cancellationToken);
        }

        public async Task<IEnumerable<GroupDto>> GetAdminGroups(CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetAdminGroupsQuery(), cancellationToken);
        }

        public async Task<IEnumerable<GroupDto>> GetGroupsByStatus(string status, CancellationToken cancellationToken)
        {
            return await _mediator.Send(new GetGroupsByStatusQuery(status), cancellationToken);
        }
    }
}
