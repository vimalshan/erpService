using MediatR;
using ProxyModule.Application.DTOs;

namespace ProxyModule.Application.Commands.CreateProxyRight;

public sealed record CreateProxyRightCommand(
    long ProxyUserId,
    long DelegatedUserId,
    DateTime ProxyStartDate,
    DateTime? ProxyEndDate,
    string ProxyType,
    string? Scope,
    string? Notes,
    long CreatedBy
) : IRequest<ProxyRightDto>;
