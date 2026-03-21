using TourPlanService.Domain.Entities;

namespace TourPlanService.Domain.Interfaces;

public interface ITourPlanRepository
{
    Task<TourPlan?> GetByIdAsync(string tpId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TourPlan>> GetByEmployeeIdAsync(string empSysId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TourPlan>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<int> GetCountAsync(CancellationToken cancellationToken = default);
    Task AddAsync(TourPlan tourPlan, CancellationToken cancellationToken = default);
    void Update(TourPlan tourPlan);
    void Delete(TourPlan tourPlan);
    Task<bool> ExistsAsync(string tpId, CancellationToken cancellationToken = default);
}

public interface IForexRepository
{
    Task<ForexRequisition?> GetByIdAsync(string forReqId, CancellationToken cancellationToken = default);
    Task<IEnumerable<ForexRequisition>> GetByTourPlanIdAsync(string tpId, CancellationToken cancellationToken = default);
    Task AddAsync(ForexRequisition forex, CancellationToken cancellationToken = default);
    void Update(ForexRequisition forex);
}
