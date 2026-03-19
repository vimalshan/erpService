using ClubMembershipService.Domain.Entities;

namespace ClubMembershipService.Domain.Interfaces;

public interface IClubActivityRepository
{
    Task<ClubActivity?> GetByIdAsync(long activityId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClubActivity>> GetByClubIdAsync(long clubId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ClubActivity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ClubActivity> AddAsync(ClubActivity activity, CancellationToken cancellationToken = default);
    Task UpdateAsync(ClubActivity activity, CancellationToken cancellationToken = default);
}
