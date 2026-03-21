using AutoMapper;
using MediatR;
using TourPlanService.Application.Common;
using TourPlanService.Application.DTOs;
using TourPlanService.Domain.Interfaces;

namespace TourPlanService.Application.Queries.GetTourPlanList;

public sealed class GetTourPlanListQueryHandler(
    ITourPlanRepository repository,
    IMapper mapper) : IRequestHandler<GetTourPlanListQuery, PaginatedList<TourPlanSummaryDto>>
{
    public async Task<PaginatedList<TourPlanSummaryDto>> Handle(
        GetTourPlanListQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.TourPlan> plans;

        if (!string.IsNullOrWhiteSpace(request.EmployeeId))
            plans = await repository.GetByEmployeeIdAsync(request.EmployeeId, cancellationToken);
        else
            plans = await repository.GetAllAsync(request.PageNumber, request.PageSize, cancellationToken);

        if (!string.IsNullOrWhiteSpace(request.Status))
            plans = plans.Where(p => p.TpStatus.Equals(request.Status, StringComparison.OrdinalIgnoreCase));

        var totalCount = await repository.GetCountAsync(cancellationToken);
        var dtos = mapper.Map<IEnumerable<TourPlanSummaryDto>>(plans).ToList();

        return new PaginatedList<TourPlanSummaryDto>(dtos, totalCount, request.PageNumber, request.PageSize);
    }
}
