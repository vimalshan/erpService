using MediatR;
using ClubMembershipService.Application.DTOs;
using ClubMembershipService.Domain.Interfaces;

namespace ClubMembershipService.Application.Queries.GetClubs;

public class GetClubsQueryHandler :
    IRequestHandler<GetClubsQuery, IEnumerable<ClubDto>>,
    IRequestHandler<GetClubByIdQuery, ClubDto?>
{
    private readonly IClubRepository _clubRepository;

    public GetClubsQueryHandler(IClubRepository clubRepository)
        => _clubRepository = clubRepository;

    public async Task<IEnumerable<ClubDto>> Handle(GetClubsQuery request, CancellationToken cancellationToken)
    {
        var clubs = request.ActiveOnly
            ? await _clubRepository.GetActiveAsync(cancellationToken)
            : await _clubRepository.GetAllAsync(cancellationToken);

        return clubs.Select(c => new ClubDto(
            c.ClubId, c.ClubName, c.Status.Value,
            c.CreatedBy, c.CreatedOn, c.ModifiedBy, c.ModifiedOn));
    }

    public async Task<ClubDto?> Handle(GetClubByIdQuery request, CancellationToken cancellationToken)
    {
        var club = await _clubRepository.GetByIdAsync(request.ClubId, cancellationToken);
        return club is null ? null : new ClubDto(
            club.ClubId, club.ClubName, club.Status.Value,
            club.CreatedBy, club.CreatedOn, club.ModifiedBy, club.ModifiedOn);
    }
}
