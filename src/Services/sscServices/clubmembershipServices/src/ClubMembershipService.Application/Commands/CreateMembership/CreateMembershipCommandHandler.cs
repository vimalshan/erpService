using MediatR;
using ClubMembershipService.Application.DTOs;
using ClubMembershipService.Domain.Entities;
using ClubMembershipService.Domain.Exceptions;
using ClubMembershipService.Domain.Interfaces;

namespace ClubMembershipService.Application.Commands.CreateMembership;

public class CreateMembershipCommandHandler : IRequestHandler<CreateMembershipCommand, MembershipDto>
{
    private readonly IClubRepository _clubRepository;
    private readonly IClubMembershipRepository _membershipRepository;

    public CreateMembershipCommandHandler(
        IClubRepository clubRepository,
        IClubMembershipRepository membershipRepository)
    {
        _clubRepository = clubRepository;
        _membershipRepository = membershipRepository;
    }

    public async Task<MembershipDto> Handle(CreateMembershipCommand request, CancellationToken cancellationToken)
    {
        var club = await _clubRepository.GetByIdAsync(request.ClubId, cancellationToken)
            ?? throw new ClubNotFoundException(request.ClubId);

        var alreadyExists = await _membershipRepository.ExistsActiveAsync(
            request.ClubId, request.MemberId, cancellationToken);
        if (alreadyExists)
            throw new DuplicateMembershipException(request.ClubId, request.MemberId);

        var membership = ClubMembership.Create(
            request.ClubId, request.MemberId, request.JoinDate,
            request.MembershipFee, request.EnrolledBy);

        var saved = await _membershipRepository.AddAsync(membership, cancellationToken);

        return new MembershipDto(
            saved.MembershipId, saved.ClubId, club.ClubName, saved.MemberId,
            saved.JoinDate, saved.MembershipFee, saved.Status.Value,
            saved.CreatedBy, saved.CreatedOn, saved.ModifiedBy, saved.ModifiedOn);
    }
}
