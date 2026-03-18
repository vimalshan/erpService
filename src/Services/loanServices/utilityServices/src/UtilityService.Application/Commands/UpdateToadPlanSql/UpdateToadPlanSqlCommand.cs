using MediatR;

namespace UtilityService.Application.Commands.UpdateToadPlanSql;

public record UpdateToadPlanSqlCommand(
    int Id,
    string? Username,
    string? Statement,
    DateTime? Timestamp
) : IRequest<bool>;
