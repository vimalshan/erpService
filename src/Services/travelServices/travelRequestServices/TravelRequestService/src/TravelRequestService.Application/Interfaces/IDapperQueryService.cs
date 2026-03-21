using TravelRequestService.Application.DTOs;

namespace TravelRequestService.Application.Interfaces;

public interface IDapperQueryService
{
    Task<IReadOnlyList<DashTourPlanDto>> GetDashTourPlansAsync(CancellationToken cancellationToken = default);
    Task<TravelRequestDto?> GetTravelRequestDetailsAsync(long travelReqId, CancellationToken cancellationToken = default);
}
