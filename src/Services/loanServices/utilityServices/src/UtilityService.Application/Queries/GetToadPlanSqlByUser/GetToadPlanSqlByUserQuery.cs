using MediatR;
using UtilityService.Application.DTOs;

namespace UtilityService.Application.Queries.GetToadPlanSqlByUser;

public record GetToadPlanSqlByUserQuery(string Username) : IRequest<IEnumerable<ToadPlanSqlDto>>;
