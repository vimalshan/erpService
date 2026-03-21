using MediatR;
using TravelService.Application.DTOs;
using TravelService.Domain.Repositories;

namespace TravelService.Application.TourPlans.Queries.GetTourPlanList;

public record GetTourPlanListQuery(int Page = 1, int PageSize = 20, string? EmployeeSysId = null) : IRequest<PagedResult<TourPlanDto>>;

public class GetTourPlanListHandler : IRequestHandler<GetTourPlanListQuery, PagedResult<TourPlanDto>>
{
    private readonly ITourPlanRepository _repository;

    public GetTourPlanListHandler(ITourPlanRepository repository) => _repository = repository;

    public async Task<PagedResult<TourPlanDto>> Handle(GetTourPlanListQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Domain.Entities.TourPlan.TourPlan> items;
        int total;

        if (!string.IsNullOrEmpty(request.EmployeeSysId))
        {
            items = await _repository.GetByEmployeeAsync(request.EmployeeSysId, cancellationToken);
            total = items.Count();
            items = items.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);
        }
        else
        {
            total = await _repository.CountAsync(cancellationToken);
            items = await _repository.GetAllAsync(request.Page, request.PageSize, cancellationToken);
        }

        return new PagedResult<TourPlanDto>
        {
            Items = items.Select(tp => new TourPlanDto
            {
                Id = tp.Id,
                EmployeeSysId = tp.EmployeeSysId,
                StartDate = tp.StartDate,
                EndDate = tp.EndDate,
                Purpose = tp.Purpose,
                Status = tp.Status,
                Category = tp.Category,
                CreatedBy = tp.CreatedBy,
                CreatedOn = tp.CreatedOn,
                FromCityName = tp.FromCity.CityName,
                ToCityName = tp.ToCity.CityName
            }).ToList(),
            TotalCount = total,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
