using MediatR;
using ClubMembershipService.Application.DTOs;

namespace ClubMembershipService.Application.Queries.GetMemberships;

public record GetMembershipsByClubQuery(long ClubId) : IRequest<IEnumerable<MembershipDto>>;
public record GetMembershipsByMemberQuery(long MemberId) : IRequest<IEnumerable<MembershipDto>>;
public record GetMembershipByIdQuery(long MembershipId) : IRequest<MembershipDto?>;
