using MediatR;
using ClubMembershipService.Application.DTOs;

namespace ClubMembershipService.Application.Queries.GetClubs;

public record GetClubsQuery(bool ActiveOnly = false) : IRequest<IEnumerable<ClubDto>>;

public record GetClubByIdQuery(long ClubId) : IRequest<ClubDto?>;
