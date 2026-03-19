using ClubMembershipService.Domain.Entities;

namespace ClubMembershipService.Domain.Interfaces;

public interface IClubRepository
{
    Task<ClubMaster?> GetByIdAsync(long clubId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClubMaster>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<ClubMaster>> GetActiveAsync(CancellationToken cancellationToken = default);
    Task<ClubMaster> AddAsync(ClubMaster club, CancellationToken cancellationToken = default);
    Task UpdateAsync(ClubMaster club, CancellationToken cancellationToken = default);
    Task<int> GetActiveCountAsync(CancellationToken cancellationToken = default);
}
