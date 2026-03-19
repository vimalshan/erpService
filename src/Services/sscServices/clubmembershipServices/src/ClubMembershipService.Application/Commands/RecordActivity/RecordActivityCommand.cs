using MediatR;
using ClubMembershipService.Application.DTOs;

namespace ClubMembershipService.Application.Commands.RecordActivity;

public record RecordActivityCommand(
    long ClubId,
    string ActivityName,
    DateOnly ActivityDate,
    decimal? Budget,
    long OrganizerId) : IRequest<ActivityDto>;
