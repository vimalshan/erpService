using MediatR;
using ProxyModule.Application.DTOs;

namespace ProxyModule.Application.Queries.GetProxyRightById;

public sealed record GetProxyRightByIdQuery(long ProxyId) : IRequest<ProxyRightDto?>;
