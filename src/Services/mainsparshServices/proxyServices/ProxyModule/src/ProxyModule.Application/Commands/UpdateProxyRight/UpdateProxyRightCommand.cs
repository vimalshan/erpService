using MediatR;
using ProxyModule.Application.DTOs;

namespace ProxyModule.Application.Commands.UpdateProxyRight;

public sealed record UpdateProxyRightCommand(
    long ProxyId,
    DateTime? ProxyStartDate,
    DateTime? ProxyEndDate,
    string? ProxyType,
    string? Scope,
    string? Notes,
    long UpdatedBy
) : IRequest<ProxyRightDto>;
