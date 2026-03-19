using MediatR;
using ClubMembershipService.Application.DTOs;

namespace ClubMembershipService.Application.Commands.CreateMembership;

public record CreateMembershipCommand(
    long ClubId,
    long MemberId,
    DateOnly JoinDate,
    decimal? MembershipFee,
    long EnrolledBy) : IRequest<MembershipDto>;
