using MediatR;
using ClubMembershipService.Application.DTOs;
using ClubMembershipService.Domain.Entities;
using ClubMembershipService.Domain.Interfaces;

namespace ClubMembershipService.Application.Commands.CreateClub;

public class CreateClubCommandHandler : IRequestHandler<CreateClubCommand, ClubDto>
{
    private readonly IClubRepository _clubRepository;

    public CreateClubCommandHandler(IClubRepository clubRepository)
        => _clubRepository = clubRepository;

    public async Task<ClubDto> Handle(CreateClubCommand request, CancellationToken cancellationToken)
    {
        var club = ClubMaster.Create(request.ClubName, request.CreatedBy);
        var saved = await _clubRepository.AddAsync(club, cancellationToken);

        return new ClubDto(
            saved.ClubId, saved.ClubName, saved.Status.Value,
            saved.CreatedBy, saved.CreatedOn,
            saved.ModifiedBy, saved.ModifiedOn);
    }
}
