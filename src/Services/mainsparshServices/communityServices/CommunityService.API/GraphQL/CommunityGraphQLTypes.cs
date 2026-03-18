namespace CommunityService.API.GraphQL;

using HotChocolate.Types;
using MediatR;
using Application.DTOs;

public class CommunityGraphQLType
{
    public long CommunityId { get; set; }
    public string CommunityCode { get; set; } = default!;
    public string CommunityName { get; set; } = default!;
    public string? CommunityDescription { get; set; }
    public string CommunityType { get; set; } = default!;
    public string PrivacyLevel { get; set; } = default!;
    public long OwnerId { get; set; }
    public string CommunityStatus { get; set; } = default!;
    public int MemberCount { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public List<CommunityMemberGraphQLType> Members { get; set; } = new();
}

public class CommunityMemberGraphQLType
{
    public long MemberId { get; set; }
    public long CommunityId { get; set; }
    public long UserSysId { get; set; }
    public string MemberRole { get; set; } = default!;
    public DateTime JoinDate { get; set; }
    public DateTime? LeaveDate { get; set; }
    public string MemberStatus { get; set; } = default!;
    public int ContributionCount { get; set; }
}

public class Query
{
    public async Task<CommunityGraphQLType?> GetCommunity(
        [Service] IMediator mediator,
        long id)
    {
        var result = await mediator.Send(new Application.Queries.GetCommunityByIdQuery(id));
        
        if (result == null)
            return null;

        return new CommunityGraphQLType
        {
            CommunityId = result.CommunityId,
            CommunityCode = result.CommunityCode,
            CommunityName = result.CommunityName,
            CommunityDescription = result.CommunityDescription,
            CommunityType = result.CommunityType,
            PrivacyLevel = result.PrivacyLevel,
            OwnerId = result.OwnerId,
            CommunityStatus = result.CommunityStatus,
            MemberCount = result.MemberCount,
            CreatedOn = result.CreatedOn,
            UpdatedOn = result.UpdatedOn
        };
    }

    public async Task<List<CommunityGraphQLType>> GetCommunities(
        [Service] IMediator mediator,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var results = await mediator.Send(new Application.Queries.GetAllCommunitiesQuery(pageNumber, pageSize));
        return results.Select(r => new CommunityGraphQLType
        {
            CommunityId = r.CommunityId,
            CommunityCode = r.CommunityCode,
            CommunityName = r.CommunityName,
            CommunityType = r.CommunityType,
            PrivacyLevel = r.PrivacyLevel,
            MemberCount = r.MemberCount
        }).ToList();
    }
}
