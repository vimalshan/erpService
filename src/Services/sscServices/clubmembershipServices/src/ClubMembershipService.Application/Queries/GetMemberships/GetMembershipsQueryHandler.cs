using MediatR;
using ClubMembershipService.Application.DTOs;
using ClubMembershipService.Domain.Interfaces;

namespace ClubMembershipService.Application.Queries.GetMemberships;

public class GetMembershipsQueryHandler :
    IRequestHandler<GetMembershipsByClubQuery, IEnumerable<MembershipDto>>,
    IRequestHandler<GetMembershipsByMemberQuery, IEnumerable<MembershipDto>>,
    IRequestHandler<GetMembershipByIdQuery, MembershipDto?>
{
    private readonly IClubMembershipRepository _repo;
    private readonly IClubRepository _clubRepo;

    public GetMembershipsQueryHandler(IClubMembershipRepository repo, IClubRepository clubRepo)
    {
        _repo = repo;
        _clubRepo = clubRepo;
    }

    public async Task<IEnumerable<MembershipDto>> Handle(GetMembershipsByClubQuery request, CancellationToken ct)
    {
        var items = await _repo.GetByClubIdAsync(request.ClubId, ct);
        var club = await _clubRepo.GetByIdAsync(request.ClubId, ct);
        return items.Select(m => MapDto(m, club?.ClubName));
    }

    public async Task<IEnumerable<MembershipDto>> Handle(GetMembershipsByMemberQuery request, CancellationToken ct)
    {
        var items = await _repo.GetByMemberIdAsync(request.MemberId, ct);
        var clubIds = items.Select(m => m.ClubId).Distinct();
        var clubs = new Dictionary<long, string>();
        foreach (var cid in clubIds)
        {
            var club = await _clubRepo.GetByIdAsync(cid, ct);
            if (club is not null) clubs[cid] = club.ClubName;
        }
        return items.Select(m => MapDto(m, clubs.GetValueOrDefault(m.ClubId)));
    }

    public async Task<MembershipDto?> Handle(GetMembershipByIdQuery request, CancellationToken ct)
    {
        var m = await _repo.GetByIdAsync(request.MembershipId, ct);
        if (m is null) return null;
        var club = await _clubRepo.GetByIdAsync(m.ClubId, ct);
        return MapDto(m, club?.ClubName);
    }

    private static MembershipDto MapDto(Domain.Entities.ClubMembership m, string? clubName) =>
        new(m.MembershipId, m.ClubId, clubName, m.MemberId,
            m.JoinDate, m.MembershipFee, m.Status.Value,
            m.CreatedBy, m.CreatedOn, m.ModifiedBy, m.ModifiedOn);
}
