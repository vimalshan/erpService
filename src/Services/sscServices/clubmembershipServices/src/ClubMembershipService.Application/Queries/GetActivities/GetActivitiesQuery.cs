using MediatR;
using ClubMembershipService.Application.DTOs;

namespace ClubMembershipService.Application.Queries.GetActivities;

public record GetActivitiesByClubQuery(long ClubId) : IRequest<IEnumerable<ActivityDto>>;
public record GetAllActivitiesQuery() : IRequest<IEnumerable<ActivityDto>>;
public record GetActivityByIdQuery(long ActivityId) : IRequest<ActivityDto?>;
