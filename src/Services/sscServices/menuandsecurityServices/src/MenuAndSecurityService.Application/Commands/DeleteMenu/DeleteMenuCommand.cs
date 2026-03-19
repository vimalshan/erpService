using MediatR;

namespace MenuAndSecurityService.Application.Commands.DeleteMenu;

public sealed record DeleteMenuCommand(long MenuId) : IRequest<bool>;
