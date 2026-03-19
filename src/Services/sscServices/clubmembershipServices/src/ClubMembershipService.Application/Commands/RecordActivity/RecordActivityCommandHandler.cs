using MediatR;
using ClubMembershipService.Application.DTOs;
using ClubMembershipService.Domain.Entities;
using ClubMembershipService.Domain.Exceptions;
using ClubMembershipService.Domain.Interfaces;

namespace ClubMembershipService.Application.Commands.RecordActivity;

public class RecordActivityCommandHandler : IRequestHandler<RecordActivityCommand, ActivityDto>
{
    private readonly IClubRepository _clubRepository;
    private readonly IClubActivityRepository _activityRepository;

    public RecordActivityCommandHandler(
        IClubRepository clubRepository,
        IClubActivityRepository activityRepository)
    {
        _clubRepository = clubRepository;
        _activityRepository = activityRepository;
    }

    public async Task<ActivityDto> Handle(RecordActivityCommand request, CancellationToken cancellationToken)
    {
        var club = await _clubRepository.GetByIdAsync(request.ClubId, cancellationToken)
            ?? throw new ClubNotFoundException(request.ClubId);

        var activity = ClubActivity.Create(
            request.ClubId, request.ActivityName, request.ActivityDate,
            request.Budget, request.OrganizerId);

        var saved = await _activityRepository.AddAsync(activity, cancellationToken);

        return new ActivityDto(
            saved.ActivityId, saved.ClubId, club.ClubName,
            saved.ActivityName, saved.ActivityDate, saved.ActivityBudget,
            saved.OrganizerId, saved.Status.Value,
            saved.CreatedBy, saved.CreatedOn, saved.ModifiedBy, saved.ModifiedOn);
    }
}
