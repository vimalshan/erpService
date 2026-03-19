using MediatR;
using HotChocolate;
using ClubMembershipService.Application.DTOs;
using ClubMembershipService.Application.Queries.GetClubs;
using ClubMembershipService.Application.Queries.GetMemberships;
using ClubMembershipService.Application.Queries.GetActivities;

namespace ClubMembershipService.API.GraphQL;

public class Query
{
    public async Task<IEnumerable<ClubDto>> GetClubs(
        [Service] IMediator mediator,
        bool activeOnly = false,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetClubsQuery(activeOnly), cancellationToken);

    public async Task<ClubDto?> GetClubById(
        [Service] IMediator mediator,
        long clubId,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetClubByIdQuery(clubId), cancellationToken);

    public async Task<IEnumerable<MembershipDto>> GetMembershipsByClub(
        [Service] IMediator mediator,
        long clubId,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetMembershipsByClubQuery(clubId), cancellationToken);

    public async Task<IEnumerable<MembershipDto>> GetMembershipsByMember(
        [Service] IMediator mediator,
        long memberId,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetMembershipsByMemberQuery(memberId), cancellationToken);

    public async Task<IEnumerable<ActivityDto>> GetActivitiesByClub(
        [Service] IMediator mediator,
        long clubId,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetActivitiesByClubQuery(clubId), cancellationToken);

    public async Task<IEnumerable<ActivityDto>> GetAllActivities(
        [Service] IMediator mediator,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetAllActivitiesQuery(), cancellationToken);
}

