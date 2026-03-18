using MediatR;
using UtilityService.Application.DTOs;

namespace UtilityService.Application.Queries.GetAllToadPlanSql;

public record GetAllToadPlanSqlQuery(int PageNumber = 1, int PageSize = 20) : IRequest<PagedResultDto<ToadPlanSqlDto>>;
