namespace CommunityService.Application.Queries;

using MediatR;
using DTOs;

public record GetCommunityByIdQuery(long CommunityId) : IRequest<CommunityDto?>;

public record GetAllCommunitiesQuery(int PageNumber = 1, int PageSize = 10) : IRequest<IEnumerable<CommunityListDto>>;

public record GetCommunitiesByTypeQuery(string Type, int PageNumber = 1, int PageSize = 10) : IRequest<IEnumerable<CommunityListDto>>;

public record GetCommunitiesByOwnerQuery(long OwnerId, int PageNumber = 1, int PageSize = 10) : IRequest<IEnumerable<CommunityListDto>>;

public record GetCommunityMembersQuery(long CommunityId) : IRequest<IEnumerable<CommunityMemberDto>>;

public record GetCommunityMemberQuery(long CommunityId, long UserId) : IRequest<CommunityMemberDto?>;

public record SearchCommunitiesQuery(string SearchTerm, int PageNumber = 1, int PageSize = 10) : IRequest<IEnumerable<CommunityListDto>>;
