using TravelRequestService.Domain.Entities;

namespace TravelRequestService.Domain.Interfaces;

public interface ITravelRequestRepository
{
    Task<TravelMain?> GetByIdAsync(long planNumber, string companyCode, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TravelMain>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TravelMain>> GetByUserAsync(long userNumber, CancellationToken cancellationToken = default);
    Task AddAsync(TravelMain travelMain, CancellationToken cancellationToken = default);
    Task UpdateAsync(TravelMain travelMain, CancellationToken cancellationToken = default);
    Task DeleteAsync(long planNumber, string companyCode, CancellationToken cancellationToken = default);
}
