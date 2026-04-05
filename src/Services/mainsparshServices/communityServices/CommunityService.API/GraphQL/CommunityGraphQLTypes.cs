namespace CommunityService.API.GraphQL;

using HotChocolate.Types;
using MediatR;
using CommunityService.Application.Commands;
using CommunityService.Application.DTOs;

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

public class Mutation
{
    public async Task<CommunityGraphQLType> CreateCommunity(
        [Service] IMediator mediator,
        string communityCode,
        string communityName,
        string? communityDescription,
        string communityType,
        string privacyLevel,
        long ownerId,
        string? communityIcon = null,
        string? communityBanner = null)
    {
        var dto = new CreateCommunityDto(communityCode, communityName, communityDescription,
            communityType, communityIcon, communityBanner, privacyLevel, ownerId);
        var result = await mediator.Send(new CreateCommunityCommand(dto));
        return MapToCommunityGraphQLType(result);
    }

    public async Task<CommunityGraphQLType> UpdateCommunity(
        [Service] IMediator mediator,
        long communityId,
        string communityName,
        string privacyLevel,
        string? communityDescription = null)
    {
        var dto = new UpdateCommunityDto(communityId, communityName, communityDescription, privacyLevel);
        var result = await mediator.Send(new UpdateCommunityCommand(dto));
        return MapToCommunityGraphQLType(result);
    }

    public async Task<bool> ArchiveCommunity(
        [Service] IMediator mediator,
        long communityId)
    {
        return await mediator.Send(new ArchiveCommunityCommand(communityId));
    }

    public async Task<CommunityMemberGraphQLType> AddMember(
        [Service] IMediator mediator,
        long communityId,
        long userId,
        string memberRole)
    {
        var dto = new AddMemberDto(communityId, userId, memberRole);
        var result = await mediator.Send(new AddCommunityMemberCommand(dto));
        return MapToMemberGraphQLType(result);
    }

    public async Task<bool> RemoveMember(
        [Service] IMediator mediator,
        long communityId,
        long userId)
    {
        var dto = new RemoveMemberDto(communityId, userId);
        return await mediator.Send(new RemoveCommunityMemberCommand(dto));
    }

    public async Task<CommunityMemberGraphQLType> ChangeMemberRole(
        [Service] IMediator mediator,
        long communityId,
        long userId,
        string newRole)
    {
        var dto = new ChangeMemberRoleDto(communityId, userId, newRole);
        var result = await mediator.Send(new ChangeMemberRoleCommand(dto));
        return MapToMemberGraphQLType(result);
    }

    private static CommunityGraphQLType MapToCommunityGraphQLType(CommunityDto r) => new()
    {
        CommunityId = r.CommunityId,
        CommunityCode = r.CommunityCode,
        CommunityName = r.CommunityName,
        CommunityDescription = r.CommunityDescription,
        CommunityType = r.CommunityType,
        PrivacyLevel = r.PrivacyLevel,
        OwnerId = r.OwnerId,
        CommunityStatus = r.CommunityStatus,
        MemberCount = r.MemberCount,
        CreatedOn = r.CreatedOn,
        UpdatedOn = r.UpdatedOn
    };

    private static CommunityMemberGraphQLType MapToMemberGraphQLType(CommunityMemberDto m) => new()
    {
        MemberId = m.MemberId,
        CommunityId = m.CommunityId,
        UserSysId = m.UserSysId,
        MemberRole = m.MemberRole,
        JoinDate = m.JoinDate,
        LeaveDate = m.LeaveDate,
        MemberStatus = m.MemberStatus,
        ContributionCount = m.ContributionCount
    };
}
