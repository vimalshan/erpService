using TravelService.Domain.Entities.TourPlan;

namespace TravelService.Domain.Repositories;

public interface ITourPlanRepository
{
    Task<TourPlan?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TourPlan>> GetByEmployeeAsync(string employeeSysId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TourPlan>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<TourPlan> AddAsync(TourPlan tourPlan, CancellationToken cancellationToken = default);
    Task UpdateAsync(TourPlan tourPlan, CancellationToken cancellationToken = default);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
}
