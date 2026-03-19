using MediatR;

namespace MenuAndSecurityService.Application.Commands.RevokeMenuAccess;

public sealed record RevokeMenuAccessCommand(long MenuAccessId) : IRequest<bool>;
