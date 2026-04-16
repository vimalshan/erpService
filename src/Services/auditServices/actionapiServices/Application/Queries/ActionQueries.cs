using ActionService.Application.DTOs;
using MediatR;

namespace ActionService.Application.Queries;

public record GetActionByIdQuery(int Id) : IRequest<ActionDto?>;
public record GetAllActionsQuery() : IRequest<IEnumerable<ActionDto>>;
public record GetActionsByEntityQuery(string EntityType, int EntityId) : IRequest<IEnumerable<ActionDto>>;
