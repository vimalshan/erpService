using MediatR;
using ClubMembershipService.Application.DTOs;
using ClubMembershipService.Domain.Interfaces;

namespace ClubMembershipService.Application.Queries.GetActivities;

public class GetActivitiesQueryHandler :
    IRequestHandler<GetActivitiesByClubQuery, IEnumerable<ActivityDto>>,
    IRequestHandler<GetAllActivitiesQuery, IEnumerable<ActivityDto>>,
    IRequestHandler<GetActivityByIdQuery, ActivityDto?>
{
    private readonly IClubActivityRepository _repo;
    private readonly IClubRepository _clubRepo;

    public GetActivitiesQueryHandler(IClubActivityRepository repo, IClubRepository clubRepo)
    {
        _repo = repo;
        _clubRepo = clubRepo;
    }

    public async Task<IEnumerable<ActivityDto>> Handle(GetActivitiesByClubQuery request, CancellationToken ct)
    {
        var items = await _repo.GetByClubIdAsync(request.ClubId, ct);
        var club = await _clubRepo.GetByIdAsync(request.ClubId, ct);
        return items.Select(a => MapDto(a, club?.ClubName));
    }

    public async Task<IEnumerable<ActivityDto>> Handle(GetAllActivitiesQuery request, CancellationToken ct)
    {
        var items = await _repo.GetAllAsync(ct);
        var clubIds = items.Select(a => a.ClubId).Distinct();
        var clubs = new Dictionary<long, string>();
        foreach (var cid in clubIds)
        {
            var club = await _clubRepo.GetByIdAsync(cid, ct);
            if (club is not null) clubs[cid] = club.ClubName;
        }
        return items.Select(a => MapDto(a, clubs.GetValueOrDefault(a.ClubId)));
    }

    public async Task<ActivityDto?> Handle(GetActivityByIdQuery request, CancellationToken ct)
    {
        var a = await _repo.GetByIdAsync(request.ActivityId, ct);
        if (a is null) return null;
        var club = await _clubRepo.GetByIdAsync(a.ClubId, ct);
        return MapDto(a, club?.ClubName);
    }

    private static ActivityDto MapDto(Domain.Entities.ClubActivity a, string? clubName) =>
        new(a.ActivityId, a.ClubId, clubName, a.ActivityName,
            a.ActivityDate, a.ActivityBudget, a.OrganizerId, a.Status.Value,
            a.CreatedBy, a.CreatedOn, a.ModifiedBy, a.ModifiedOn);
}
