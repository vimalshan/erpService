using MediatR;
using UtilityService.Application.DTOs;

namespace UtilityService.Application.Queries.GetToadPlanSqlById;

public record GetToadPlanSqlByIdQuery(int Id) : IRequest<ToadPlanSqlDto?>;
