using MediatR;
using ProxyModule.Application.DTOs;

namespace ProxyModule.Application.Queries.GetActiveProxyRights;

public sealed record GetActiveProxyRightsQuery : IRequest<IEnumerable<ProxyRightDto>>;
