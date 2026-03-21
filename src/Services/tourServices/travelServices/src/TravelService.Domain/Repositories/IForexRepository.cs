using TravelService.Domain.Entities.Forex;

namespace TravelService.Domain.Repositories;

public interface IForexRepository
{
    Task<ForexMain?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<ForexMain>> GetByTourPlanAsync(string tourPlanId, CancellationToken cancellationToken = default);
    Task<ForexMain> AddAsync(ForexMain forex, CancellationToken cancellationToken = default);
    Task UpdateAsync(ForexMain forex, CancellationToken cancellationToken = default);
}
