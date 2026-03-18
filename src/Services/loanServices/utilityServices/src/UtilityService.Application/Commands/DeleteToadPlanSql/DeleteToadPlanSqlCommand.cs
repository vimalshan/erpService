using MediatR;

namespace UtilityService.Application.Commands.DeleteToadPlanSql;

public record DeleteToadPlanSqlCommand(int Id) : IRequest<bool>;
