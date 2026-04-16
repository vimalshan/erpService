using ActionService.Application.DTOs;
using MediatR;

namespace ActionService.Application.Commands;

public record CreateActionCommand(CreateActionDto Dto) : IRequest<ActionDto>;
public record UpdateActionCommand(UpdateActionDto Dto) : IRequest<ActionDto>;
public record DeleteActionCommand(int Id) : IRequest<bool>;
public record CompleteActionCommand(int Id) : IRequest<bool>;
