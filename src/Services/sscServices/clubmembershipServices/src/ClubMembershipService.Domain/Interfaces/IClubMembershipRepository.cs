using ClubMembershipService.Domain.Entities;

namespace ClubMembershipService.Domain.Interfaces;

public interface IClubMembershipRepository
{
    Task<ClubMembership?> GetByIdAsync(long membershipId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClubMembership>> GetByClubIdAsync(long clubId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClubMembership>> GetByMemberIdAsync(long memberId, CancellationToken cancellationToken = default);
    Task<bool> ExistsActiveAsync(long clubId, long memberId, CancellationToken cancellationToken = default);
    Task<ClubMembership> AddAsync(ClubMembership membership, CancellationToken cancellationToken = default);
    Task UpdateAsync(ClubMembership membership, CancellationToken cancellationToken = default);
}
