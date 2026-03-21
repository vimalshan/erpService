using TravelService.Domain.Entities;

namespace TravelService.Domain.Repositories;

public interface IApproverDetailRepository
{
    Task<IEnumerable<ApproverDetail>> GetByTourPlanAsync(string tourPlanId, CancellationToken cancellationToken = default);
    Task<ApproverDetail> AddAsync(ApproverDetail detail, CancellationToken cancellationToken = default);
}
