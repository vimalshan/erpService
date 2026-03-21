using MediatR;
using TourPlanService.Application.Common;
using TourPlanService.Application.DTOs;

namespace TourPlanService.Application.Queries.GetTourPlanList;

public sealed record GetTourPlanListQuery(
    int PageNumber = 1,
    int PageSize = 20,
    string? EmployeeId = null,
    string? Status = null) : IRequest<PaginatedList<TourPlanSummaryDto>>;
