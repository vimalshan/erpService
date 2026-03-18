using DispatchPlanning.Application.DTOs;
using MediatR;

namespace DispatchPlanning.Application.Features.DispatchPlans.Queries;

public record GetDispatchPlanByIdQuery(int PlanHeaderId) : IRequest<DispatchPlanDetailDto?>;

public record GetAllDispatchPlansQuery(int CompanyUnitId) : IRequest<IEnumerable<DispatchPlanHeaderDto>>;

public record GetDispatchPlanItemsQuery(int PlanHeaderId) : IRequest<IEnumerable<DispatchPlanItemDto>>;

public record GetAllMainGroupsQuery(int CompanyUnitId) : IRequest<IEnumerable<MainGroupDto>>;

public record GetSubGroupsByMainGroupQuery(int MainGroupId) : IRequest<IEnumerable<SubGroupDto>>;

public record GetBreakupItemsBySubGroupQuery(int SubGroupId) : IRequest<IEnumerable<BreakupItemDto>>;
