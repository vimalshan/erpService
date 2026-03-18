using MediatR;

namespace ProxyModule.Application.Commands.DeactivateProxyRight;

public sealed record DeactivateProxyRightCommand(long ProxyId, long UpdatedBy) : IRequest<bool>;
