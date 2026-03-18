using MediatR;
using GroupManagementService.Application.DTOs;

namespace GroupManagementService.Application.Queries
{
    public class GetGroupByIdQuery : IRequest<GroupDto>
    {
        public long GroupId { get; set; }

        public GetGroupByIdQuery(long groupId)
        {
            GroupId = groupId;
        }
    }

    public class GetGroupByCodeQuery : IRequest<GroupDto>
    {
        public string GroupCode { get; set; }

        public GetGroupByCodeQuery(string groupCode)
        {
            GroupCode = groupCode;
        }
    }

    public class GetAllGroupsQuery : IRequest<IEnumerable<GroupDto>>
    {
    }

    public class GetGroupsByStatusQuery : IRequest<IEnumerable<GroupDto>>
    {
        public string Status { get; set; }

        public GetGroupsByStatusQuery(string status)
        {
            Status = status;
        }
    }

    public class SearchGroupsQuery : IRequest<IEnumerable<GroupDto>>
    {
        public string? SearchTerm { get; set; }
        public string? Status { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public SearchGroupsQuery(string? searchTerm = null, string? status = null, int pageNumber = 1, int pageSize = 10)
        {
            SearchTerm = searchTerm;
            Status = status;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }

    public class GetAdminGroupsQuery : IRequest<IEnumerable<GroupDto>>
    {
    }
}
