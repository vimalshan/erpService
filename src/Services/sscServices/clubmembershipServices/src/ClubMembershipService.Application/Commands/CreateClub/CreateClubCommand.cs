using MediatR;
using ClubMembershipService.Application.DTOs;

namespace ClubMembershipService.Application.Commands.CreateClub;

public record CreateClubCommand(string ClubName, long CreatedBy) : IRequest<ClubDto>;
