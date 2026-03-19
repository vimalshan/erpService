using MediatR;
using HotChocolate;
using ClubMembershipService.Application.Commands.CreateClub;
using ClubMembershipService.Application.Commands.CreateMembership;
using ClubMembershipService.Application.Commands.RecordActivity;
using ClubMembershipService.Application.DTOs;

namespace ClubMembershipService.API.GraphQL;

public class Mutation
{
    public async Task<ClubDto> CreateClub(
        [Service] IMediator mediator,
        string clubName,
        long createdBy,
        CancellationToken cancellationToken)
        => await mediator.Send(new CreateClubCommand(clubName, createdBy), cancellationToken);

    public async Task<MembershipDto> CreateMembership(
        [Service] IMediator mediator,
        long clubId,
        long memberId,
        DateOnly joinDate,
        decimal? membershipFee,
        long enrolledBy,
        CancellationToken cancellationToken)
        => await mediator.Send(
            new CreateMembershipCommand(clubId, memberId, joinDate, membershipFee, enrolledBy),
            cancellationToken);

    public async Task<ActivityDto> RecordActivity(
        [Service] IMediator mediator,
        long clubId,
        string activityName,
        DateOnly activityDate,
        decimal? budget,
        long organizerId,
        CancellationToken cancellationToken)
        => await mediator.Send(
            new RecordActivityCommand(clubId, activityName, activityDate, budget, organizerId),
            cancellationToken);
}
