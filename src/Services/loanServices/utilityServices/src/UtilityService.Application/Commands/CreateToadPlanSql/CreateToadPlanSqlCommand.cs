using MediatR;
using UtilityService.Application.DTOs;

namespace UtilityService.Application.Commands.CreateToadPlanSql;

public record CreateToadPlanSqlCommand(
    string? Username,
    string StatementId,
    string? Statement,
    DateTime? Timestamp
) : IRequest<ToadPlanSqlDto>;
