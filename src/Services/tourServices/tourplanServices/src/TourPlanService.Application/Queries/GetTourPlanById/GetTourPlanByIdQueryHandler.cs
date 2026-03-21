using AutoMapper;
using MediatR;
using TourPlanService.Application.DTOs;
using TourPlanService.Domain.Interfaces;

namespace TourPlanService.Application.Queries.GetTourPlanById;

public sealed class GetTourPlanByIdQueryHandler(
    ITourPlanRepository repository,
    IMapper mapper) : IRequestHandler<GetTourPlanByIdQuery, TourPlanDto?>
{
    public async Task<TourPlanDto?> Handle(GetTourPlanByIdQuery request, CancellationToken cancellationToken)
    {
        var tourPlan = await repository.GetByIdAsync(request.TpId, cancellationToken);
        return tourPlan is null ? null : mapper.Map<TourPlanDto>(tourPlan);
    }
}
