using TravelRequestService.Domain.Entities;

namespace TravelRequestService.Domain.Interfaces;

public interface ITravelAdvanceRepository
{
    Task<IReadOnlyList<TravelAdvance>> GetByRequestAsync(long requestNumber, CancellationToken cancellationToken = default);
    Task AddAsync(TravelAdvance advance, CancellationToken cancellationToken = default);
    Task UpdateAsync(TravelAdvance advance, CancellationToken cancellationToken = default);
}
